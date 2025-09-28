using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace AlphaAPI.Helper
{
    public class ClerkService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public ClerkService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;

            // Header Authorization với SecretKey
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["Clerk:SecretKey"]}");
        }

        /// <summary>
        /// Tạo hoặc lấy user trên Clerk
        /// </summary>
        public async Task<string> EnsureUserAsync(string email, string fullName, string phoneOrPassword)
        {
            try
            {
                return await GetUserIdByEmailAsync(email);
            }
            catch
            {
                // Sử dụng email làm password tạm thời nếu không có password thực
                var tempPassword = string.IsNullOrEmpty(phoneOrPassword) ? email + "123!" : phoneOrPassword;
                return await CreateUserAsync(email, fullName, tempPassword);
            }
        }

        /// <summary>
        /// Tạo user mới trên Clerk
        /// </summary>
        public async Task<string> CreateUserAsync(string email, string fullName, string password)
        {
            var names = fullName?.Split(' ', 2) ?? new[] { email };
            var payload = new
            {
                email_address = new[] { email }, // Clerk expects array of strings
                password = password,
                first_name = names.Length > 0 ? names[0] : fullName ?? "",
                last_name = names.Length > 1 ? names[1] : ""
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://api.clerk.dev/v1/users", content);
            var rawContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Clerk create user failed: {response.StatusCode}, {rawContent}");
            }

            using var jsonDoc = JsonDocument.Parse(rawContent);
            var root = jsonDoc.RootElement;

            if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("id", out var idProp))
            {
                return idProp.GetString()!;
            }

            throw new Exception($"Unexpected JSON response from Clerk when creating user: {rawContent}");
        }

        /// <summary>
        /// Lấy user_id trên Clerk theo email
        /// </summary>
        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var encodedEmail = Uri.EscapeDataString(email);
            var response = await _http.GetAsync($"https://api.clerk.dev/v1/users?email_address={encodedEmail}");
            var rawContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Clerk get user by email failed: {response.StatusCode}, {rawContent}");
            }

            using var jsonDoc = JsonDocument.Parse(rawContent);
            var root = jsonDoc.RootElement;

            // Handle both array and object responses
            if (root.ValueKind == JsonValueKind.Array)
            {
                if (root.GetArrayLength() > 0 && root[0].TryGetProperty("id", out var idProp))
                {
                    return idProp.GetString()!;
                }
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("data", out var dataArray) &&
                    dataArray.ValueKind == JsonValueKind.Array &&
                    dataArray.GetArrayLength() > 0 &&
                    dataArray[0].TryGetProperty("id", out var idProp))
                {
                    return idProp.GetString()!;
                }
            }

            throw new Exception($"No Clerk user found with email {email}: {rawContent}");
        }

        /// <summary>
        /// Tạo session cho user trên Clerk
        /// </summary>
        public async Task<string> CreateSessionForUserAsync(string userId)
        {
            var payload = new { user_id = userId };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://api.clerk.dev/v1/sessions", content);
            var rawContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Clerk create session failed: {response.StatusCode}, {rawContent}");

            using var jsonDoc = JsonDocument.Parse(rawContent);
            var root = jsonDoc.RootElement;

            if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("id", out var idProp))
            {
                return idProp.GetString()!;
            }

            throw new Exception($"Unexpected JSON response from Clerk when creating session: {rawContent}");
        }

        /// <summary>
        /// Tạo JWT token từ sessionId
        /// </summary>
        public async Task<string> CreateSessionTokenAsync(string sessionId)
        {
            var response = await _http.PostAsync($"https://api.clerk.dev/v1/sessions/{sessionId}/tokens", null);
            var rawContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Clerk create token failed: {response.StatusCode}, {rawContent}");

            using var jsonDoc = JsonDocument.Parse(rawContent);
            var root = jsonDoc.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("jwt", out var jwtProp))
                    return jwtProp.GetString()!;
                if (root.TryGetProperty("token", out var tokenProp))
                    return tokenProp.GetString()!;
            }

            throw new Exception($"Unexpected JSON response from Clerk when creating session token: {rawContent}");
        }
    }
}
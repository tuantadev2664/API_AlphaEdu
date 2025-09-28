//using AlphaAPI.Helper;
//using BusinessObjects.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using Services.interfaces;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace AlphaAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IUserService _userService;
//        private readonly IConfiguration _config;

//        public AuthController(IUserService userService, IConfiguration config)
//        {
//            _userService = userService;
//            _config = config;
//        }


//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequest request)
//        {
//            var user = await _userService.LoginAsync(request.Email, request.Password);
//            if (user == null)
//                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });

//            // Sinh JWT
//            var token = GenerateJwtToken(user);
//            return Ok(new
//            {
//                token,
//                user = new
//                {
//                    user.Id,
//                    user.FullName,
//                    user.Email,
//                    user.Role,
//                    user.SchoolId
//                }
//            });
//        }

//        private string GenerateJwtToken(User user)
//        {
//            var jwtSettings = _config.GetSection("Jwt");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  
//        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
//        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
//        new Claim(ClaimTypes.Role, user.Role.ToLower()),
//        new Claim("FullName", user.FullName),
//        new Claim("SchoolId", user.SchoolId.ToString())
//    };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//    }
//}

//using AlphaAPI.Helper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using Services.interfaces;

//[ApiController]
//[Route("api/[controller]")]
//public class UserController : ControllerBase
//{
//    private readonly IUserService _userService;

//    public UserController(IUserService userService)
//    {
//        _userService = userService;
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] LoginRequest request)
//    {
//        var user = await _userService.LoginAsync(request.Email, request.Password);
//        if (user == null)
//            return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });

//        return Ok(new
//        {
//            message = "Login successful. Use Clerk token for API calls.",
//            user = new
//            {
//                user.Id,
//                user.FullName,
//                user.Email,
//                user.Role,
//                user.SchoolId
//            }
//        });
//    }

//    [Authorize]
//    [HttpGet("current-user")]
//    public IActionResult GetCurrentUser()
//    {
//        var identity = HttpContext.User.Identity as ClaimsIdentity;
//        if (identity == null || !identity.IsAuthenticated)
//            return Unauthorized(new { message = "Token không hợp lệ hoặc đã hết hạn" });

//        var claims = identity.Claims;

//        var user = new
//        {
//            Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
//                 ?? claims.FirstOrDefault(c => c.Type == "sub")?.Value,
//            FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
//            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
//            Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
//            SchoolId = claims.FirstOrDefault(c => c.Type == "schoolId")?.Value
//        };

//        return Ok(user);
//    }
//}

using AlphaAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Services.interfaces;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.LoginAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });

        return Ok(new
        {
            message = "Login successful. Use Clerk token for API calls.",
            user = new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Role,
                user.SchoolId
            }
        });
    }

    [Authorize]
    [HttpGet("{userId}")]
    public IActionResult GetUserById(string userId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null || !identity.IsAuthenticated)
            return Unauthorized(new { message = "Token không hợp lệ hoặc đã hết hạn" });

        var claims = identity.Claims;

        var currentUserId = claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

        if (currentUserId != userId)
        {
            return Forbid("Bạn không có quyền truy cập user này");
        }

        var user = new
        {
            Id = currentUserId,
            FullName = claims.FirstOrDefault(c => c.Type == "FullName")?.Value,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
            SchoolId = claims.FirstOrDefault(c => c.Type == "SchoolId")?.Value
        };

        return Ok(user);
    }
}

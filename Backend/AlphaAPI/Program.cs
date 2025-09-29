using AlphaAPI.Helper;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.interfaces;
using Repositories.Interfaces;
using Repositories.repositories;
using Repositories.Repositories;
using Services.interfaces;
using Services.services;
using System.Text;

namespace AlphaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

            //var envPath = Path.Combine("..", ".env");
            //DotNetEnv.Env.Load(envPath);

            configuration.AddEnvironmentVariables();

            // ------------------- Add Services -------------------
            builder.Services.AddControllers();

            // Repository & Service DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IScoreRepository, ScoreRepository>();
            builder.Services.AddScoped<IScoreServices, ScoreServices>();

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentServices, StudentServices>();

            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IMessageServices, MessageServices>();

            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            builder.Services.AddScoped<IAnnouncementServices, AnnouncementServices>();

            builder.Services.AddScoped<IBehaviorNoteRepository, BehaviorNoteRepository>();
            builder.Services.AddScoped<IBehaviorNoteServices, BehaviorNoteServices>();

            builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            builder.Services.AddScoped<IAnalyticsServices, AnalyticsServices>();

            builder.Services.AddScoped<ITeacherAssignmentRepository, TeacherAssignmentRepository>();
            builder.Services.AddScoped<ITeacherAssignmentService, TeacherAssignmentService>();

            // PostgreSQL connection
            var connectionString = configuration.GetConnectionString("MyDB");
            builder.Services.AddDbContext<SchoolDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            // JWT Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
            //        // Thêm sau phần builder.Services.AddControllers();
            //        builder.Services.AddHttpClient<ClerkService>();

            //        builder.Services.AddAuthentication("Bearer")
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = configuration["Clerk:Authority"];
            //    options.Audience = configuration["Clerk:Audience"];
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true
            //    };
            //});

            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.SetIsOriginAllowed(origin =>
                                        {
                                            // Prod domain
                                            if (origin == "https://alphaedu.id.vn" || origin == "https://api.alphaedu.id.vn")
                                                return true;

                                            // Cho phép tất cả preview deploy trên Vercel
                                            // (VD: https://alpha-edu-git-...vercel.app)
                                            if (origin.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase))
                                                return true;

                                            // Local dev
                                            if (origin == "http://localhost:3000" || origin == "http://localhost:5173")
                                                return true;

                                            return false;
                                        })
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ------------------- Build App -------------------
            var app = builder.Build();

            var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");
            if (app.Environment.IsDevelopment() || enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AlphaAPI v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();   // 🔑 thêm Authentication trước Authorization
            app.UseAuthorization();
            app.UseCors("AllowReactApp");
            app.MapControllers();
            app.Run();
        }
    }
}

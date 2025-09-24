using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.interfaces;
using Repositories.Interfaces;
using Repositories.repositories;
using Services.interfaces;
using Services.services;

namespace AlphaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            // var envPath = Path.Combine("..", ".env");
            // DotNetEnv.Env.Load(envPath);

            // Add services to the container.
            configuration.AddEnvironmentVariables();

            builder.Services.AddControllers();

            // Repository & Service DI
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

            // PostgreSQL connection
            var connectionString = configuration.GetConnectionString("MyDB");
            builder.Services.AddDbContext<SchoolDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");

            if (app.Environment.IsDevelopment() || enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // options.RoutePrefix = string.Empty; // nếu muốn / là Swagger
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AlphaAPI v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

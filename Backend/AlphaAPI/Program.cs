using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.interfaces;
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
            var connectionString = builder.Configuration.GetConnectionString("MyDB");
            builder.Services.AddDbContext<SchoolDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

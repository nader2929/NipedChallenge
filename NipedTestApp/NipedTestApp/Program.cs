using Data;
using Data.Database;
using Data.Json;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace NipedTestApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        var config = builder.Configuration;
        var settings = config.GetSection("NipedSettings").Get<ApplicationSettings>()
            ?? throw new InvalidOperationException("NipedSettings not found");
        builder.Services.AddSingleton(settings);

        builder.Services.AddPooledDbContextFactory<NipedDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(settings.NipedDatabase,
                b => { b.EnableRetryOnFailure(); });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, 1);
        
        if (settings.UseDatabase)
        {
            builder.Services.AddSingleton<IClientDataReader, DatabaseClientDataReader>();
            builder.Services.AddSingleton<IMedicalGuidelinesReader, DatabaseMedicalGuidelinesReader>();
        }
        else
        {
            builder.Services.AddSingleton<IClientDataReader, JsonClientDataReader>();
            builder.Services.AddSingleton<IMedicalGuidelinesReader, JsonMedicalGuidelinesReader>();
        }

        var app = builder.Build();
        
        // Ensure the Database has been initialized
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            if (settings.UseDatabase)
            {
                logger.LogInformation("Niped database setup");
                var dbContextFactory = services.GetRequiredService<IDbContextFactory<NipedDbContext>>();

                // Use the factory to create a DbContext instance
                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.SaveChanges();
                }  
            }
            else
            {
                logger.LogInformation("Going to get data from Json files");
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseAntiforgery();
        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}

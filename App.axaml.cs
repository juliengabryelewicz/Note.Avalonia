using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Serilog;

using Note.AppCtx;
using Note.DbContexts;
using Note.Repositories;
using Note.Services;
using Note.Views;

namespace Note
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IHostBuilder builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddSerilog(logger: LoggingCtx.LogApp, dispose: true);
                    });

                    services.AddDbContext<NoteDbContext>();

                    services.AddScoped<INoteRepository, NoteRepository>();
                    services.AddScoped<INoteService, NoteService>();
                });
    
            Program.Host = builder.Build();

            DependencyInjection.Scoped.Run((NoteDbContext dbContext) => {
                try
                {
                    dbContext.Database.Migrate();

                    LoggingCtx.LogApp.Information("Database Migration Finished");
                }
                catch (Exception ex)
                {
                    LoggingCtx.LogApp.Fatal(ex, "Issue While Running");

                    Environment.Exit(1);
                }
            });            

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
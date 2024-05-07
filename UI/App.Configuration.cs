using UI.ViewModels;
using UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Core.Services.GoogleDrive;
using Core.Settings;
using Core.Settings.Interfaces;
using Core.Services.GoogleDrive.Interfaces;
using Core.Services.GoogleDrive.Credentials;

namespace UI
{
    public partial class App
    {
        private IServiceProvider _serviceProvider;

        public static IServiceProvider Services
        {
            get
            {
                IServiceProvider serviceProvider = ((App)Current)._serviceProvider
                    ?? throw new InvalidOperationException("The service provider is not initialized");
                return serviceProvider;
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
            return services.BuildServiceProvider(true);
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            var googleDriveCredentialsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Properties/googleDriveSecrets.json");
            var googleDriveCredentials = new GoogleDriveApiFileCredentials(googleDriveCredentialsFile);
            services.AddSingleton<ISettings, LocalSettings>()
                .AddTransient<IGoogleDriveService>((_) => new GoogleDrive(googleDriveCredentials));
        }

        private static void ConfigureViewModels(ServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
        }

        private static void ConfigureViews(ServiceCollection services)
        {
            services.AddTransient<MainView>();
        }
    }
}

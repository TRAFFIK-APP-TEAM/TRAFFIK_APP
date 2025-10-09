using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.ClearProviders();
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
            
            // Create logs folder on Desktop for easy access
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var logsFolder = Path.Combine(desktopPath, "TRAFFIK_APP_Logs");
            Directory.CreateDirectory(logsFolder);
            
            var logPath = Path.Combine(logsFolder, $"debug_{DateTime.Now:yyyy-MM-dd}.log");
            System.Diagnostics.Debug.WriteLine($"LOG PATH: {logPath}");
            File.AppendAllText(logPath, $"\n\n========== App startup at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ==========\n");
            
            builder.Logging.AddProvider(new FileLoggerProvider(logPath));
            builder.Logging.SetMinimumLevel(LogLevel.Information);

#endif
            // Register routes for navigation
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(BookingPage), typeof(BookingPage));
            Routing.RegisterRoute(nameof(RewardsPage), typeof(RewardsPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(AddVehiclePage), typeof(AddVehiclePage));
            
            // API Clients with timeout configuration
            builder.Services.AddHttpClient<AuthClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
#if WINDOWS
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })
#endif
            ;
            builder.Services.AddHttpClient<UserClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<UserRoleClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<BookingClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<PaymentClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<RewardClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<NotificationClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<CarModelClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<CarTypeClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<CarTypeServicesClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<ServiceCatalogClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<ServiceHistoryClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<ReviewClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<SocialFeedClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddHttpClient<VehicleClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddSingleton<SessionService>();
            builder.Services.AddSingleton<BookingClient>();
            builder.Services.AddSingleton<RewardClient>();
            builder.Services.AddSingleton<NotificationClient>();
            builder.Services.AddSingleton<VehicleClient>();
            builder.Services.AddSingleton<CarTypeClient>();


            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignupViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<AccountViewModel>();
            builder.Services.AddTransient<AddVehicleViewModel>();
            builder.Services.AddTransient<BookingViewModel>();
            builder.Services.AddTransient<RewardsViewModel>();

            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<SignupPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<BookingPage>();
            builder.Services.AddTransient<RewardsPage>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<AddVehiclePage>();




            return builder.Build();
        }
    }
}

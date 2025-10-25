using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Helpers;

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
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(StaffDashboardPage), typeof(StaffDashboardPage));
            Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));
            Routing.RegisterRoute(nameof(BookingServiceSelectPage), typeof(BookingServiceSelectPage));
            Routing.RegisterRoute(nameof(BookingVehicleSelectPage), typeof(BookingVehicleSelectPage));
            Routing.RegisterRoute(nameof(BookingDateTimeSelectPage), typeof(BookingDateTimeSelectPage));
            Routing.RegisterRoute(nameof(BookingConfirmationPage), typeof(BookingConfirmationPage));
            


            // API Clients with timeout configuration
            builder.Services.AddHttpClient<AuthClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })
            ;
            builder.Services.AddHttpClient<UserClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<UserRoleClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<BookingClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<PaymentClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<RewardClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<RewardCatalogClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<NotificationClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<ServiceCatalogClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<ServiceHistoryClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<ReviewClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<SocialFeedClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<VehicleClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<BookingStagesClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;
            builder.Services.AddHttpClient<CarModelClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            /*.ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })*/
            ;

            builder.Services.AddSingleton<SessionService>();
            builder.Services.AddSingleton<BookingClient>();
            builder.Services.AddSingleton<RewardClient>();
            builder.Services.AddSingleton<NotificationClient>();
            builder.Services.AddSingleton<VehicleClient>();
            builder.Services.AddSingleton<CarModelClient>();
            builder.Services.AddSingleton<RewardCatalogClient>();
            builder.Services.AddSingleton<ServiceCatalogClient>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignupViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<AccountViewModel>();
            builder.Services.AddTransient<AddVehicleViewModel>();
            builder.Services.AddTransient<BookingViewModel>();
            builder.Services.AddTransient<BookingServiceSelectViewModel>();
            builder.Services.AddTransient<BookingVehicleSelectViewModel>();
            builder.Services.AddTransient<BookingDateTimeSelectViewModel>();
            builder.Services.AddTransient<BookingConfirmationViewModel>();
            builder.Services.AddTransient<RewardsViewModel>();
            builder.Services.AddTransient<StaffDashboardViewModel>();

            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<SignupPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<BookingPage>();
            builder.Services.AddTransient<BookingServiceSelectPage>();
            builder.Services.AddTransient<BookingVehicleSelectPage>();
            builder.Services.AddTransient<BookingDateTimeSelectPage>();
            builder.Services.AddTransient<BookingConfirmationPage>();
            builder.Services.AddTransient<RewardsPage>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<AddVehiclePage>();
            builder.Services.AddTransient<StaffDashboardPage>();
            builder.Services.AddTransient<AdminDashboardPage>();




            var app = builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }
    }
}

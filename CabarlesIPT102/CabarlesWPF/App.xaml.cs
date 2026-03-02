using Domain.Commands;
using Domain.Queries;
using Framework.Commands;
using Framework.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Interfaces;
using CabarlesWPF.Services;
using CabarlesWPF.Stores;
using CabarlesWPF.ViewModels;
using CabarlesWPF.Views;
using System.IO;
using System.Windows;

namespace CabarlesWPF;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection")!;

                // Repository
                services.AddSingleton<IRepository>(sp => new Repository.Repository(connectionString));

                // Commands
                services.AddTransient<ICreateStudent, CreateStudent>();
                services.AddTransient<IUpdateStudent, UpdateStudent>();
                services.AddTransient<IDeleteStudent, DeleteStudent>();

                // Queries
                services.AddTransient<IGetAllStudents, GetAllStudents>();
                services.AddTransient<IReadStudentById, ReadStudentById>();

                // Stores
                services.AddSingleton<NavigationStore>();

                // Services
                services.AddSingleton<INavigationService, NavigationService>();

                // ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<AddStudentViewModel>();

                // Views
                services.AddSingleton<MainWindow>(sp => new MainWindow
                {
                    DataContext = sp.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // Initialize database automatically
        try
        {
            var connectionString = _host.Services.GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection")!;
            var dbInitializer = new Services.DatabaseInitializer(connectionString);
            await dbInitializer.InitializeAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database initialization failed: {ex.Message}\n\nPlease check your connection string in appsettings.json", 
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        var navigationService = _host.Services.GetRequiredService<INavigationService>();
        navigationService.NavigateTo<HomeViewModel>();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}

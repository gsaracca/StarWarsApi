using Microsoft.UI.Xaml;
using StarWarsApi.Client;
using StarWarsApi.Infrastructure;
using StarWarsApi.Services;
using StarWarsApi.ViewModels;

namespace StarWarsApi;

public partial class App : Application
{
    private MainWindow? _window;

    public App() => InitializeComponent();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        InitializeLogger();

        var viewModel = BuildViewModel();

        _window = new MainWindow(viewModel);
        _window.Activate();
    }

    // ── Composition root ─────────────────────────────────────────────────────

    private static void InitializeLogger()
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StarWarsExplorer", "Logs", "app-.log");

        AppLogger.Initialize(logPath);
    }

    private static MainViewModel BuildViewModel()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(SwapiEndpoints.BaseUrl),
            Timeout     = TimeSpan.FromSeconds(30)
        };

        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "StarWarsExplorer/1.0");

        var cacheService  = new CacheService();
        var swapiClient   = new SwapiClient(httpClient, cacheService);
        var searchService = new SearchService(swapiClient);

        return new MainViewModel(searchService);
    }
}

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using StarWarsApi.Infrastructure;
using StarWarsApi.ViewModels;
using WinRT.Interop;

namespace StarWarsApi;

public sealed partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        Title = "Star Wars Explorer";
        ExtendsContentIntoTitleBar = true;
        SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };

        RootPage.ViewModel = viewModel;
        SetTitleBar(RootPage.TitleBarElement);

        ConfigureWindow();

        AppLogger.Instance.Information("MainWindow initialized");
    }

    private void ConfigureWindow()
    {
        var appWindow = GetAppWindow();
        appWindow.Resize(new Windows.Graphics.SizeInt32(1440, 900));

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = true;
            presenter.IsMaximizable = true;
            presenter.IsMinimizable = true;
            presenter.Maximize();
        }
    }

    private AppWindow GetAppWindow()
    {
        var hwnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }
}

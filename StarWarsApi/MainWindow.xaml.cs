using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
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

        SetMinimumWindowSize();

        RootPage.ViewModel = viewModel;

        AppLogger.Instance.Information("MainWindow initialized");
    }

    private void SetMinimumWindowSize()
    {
        var hwnd      = WindowNative.GetWindowHandle(this);
        var windowId  = Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        appWindow.Resize(new Windows.Graphics.SizeInt32(1280, 820));
    }
}

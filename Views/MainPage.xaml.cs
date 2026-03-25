using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using StarWarsApi.ViewModels;

namespace StarWarsApi.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; set; } = null!;
    public UIElement TitleBarElement => TitleBarDragArea;

    public MainPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SearchBox.Focus(FocusState.Programmatic);
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is string chosen)
        {
            ViewModel.SearchQuery = chosen;
        }

        _ = ViewModel.SearchCommand.ExecuteAsync(null);
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            ViewModel.SearchQuery = sender.Text;
        }
    }

    private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is string suggestion)
        {
            sender.Text = suggestion;
        }
    }

    private void ExampleChip_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Content: string query })
        {
            ViewModel.UseExampleCommand.Execute(query);
        }
    }
}

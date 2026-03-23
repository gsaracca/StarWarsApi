using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using StarWarsApi.ViewModels;

namespace StarWarsApi.Views;

public sealed partial class MainPage : Page
{
    // ViewModel is set by MainWindow after construction
    public MainViewModel ViewModel { get; set; } = null!;

    public MainPage() => InitializeComponent();

    // ── AutoSuggestBox events ────────────────────────────────────────────────

    /// <summary>Fires when the user presses Enter or selects a suggestion.</summary>
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is string chosen)
            ViewModel.SearchQuery = chosen;

        _ = ViewModel.SearchCommand.ExecuteAsync(null);
    }

    /// <summary>Keeps the ViewModel query in sync while the user types.</summary>
    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            ViewModel.SearchQuery = sender.Text;
    }

    /// <summary>Autocompletes the text box when the user picks a suggestion from the dropdown.</summary>
    private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is string suggestion)
            sender.Text = suggestion;
    }

    // ── Quick-search chip clicks ─────────────────────────────────────────────

    /// <summary>Applies the chip label as the query and immediately triggers a search.</summary>
    private void ExampleChip_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Content: string query })
            ViewModel.UseExampleCommand.Execute(query);
    }
}

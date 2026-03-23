using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarWarsApi.Infrastructure;
using StarWarsApi.Services;

namespace StarWarsApi.ViewModels;

/// <summary>
/// ViewModel for MainPage.
/// Drives the unified search UX: query debouncing, parallel fetch, state management.
/// </summary>
public sealed partial class MainViewModel : ObservableObject
{
    private readonly ISearchService        _searchService;
    private          CancellationTokenSource _cts = new();

    // ── Observable Properties ────────────────────────────────────────────────

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    [NotifyPropertyChangedFor(nameof(IsQueryEmpty))]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    private bool _hasResults;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    private bool _hasNoResults;

    [ObservableProperty]
    private string _statusMessage = "Type anything to search across the galaxy…";

    // ── Computed Properties ──────────────────────────────────────────────────

    public bool ShowWelcome  => !IsLoading && !HasResults && !HasNoResults;
    public bool IsQueryEmpty => string.IsNullOrWhiteSpace(SearchQuery);

    // ── Collections ──────────────────────────────────────────────────────────

    public ObservableCollection<SearchResultGroup> ResultGroups       { get; } = [];
    public ObservableCollection<string>            FilteredSuggestions { get; } = [];

    /// <summary>
    /// Curated example queries that guide non-expert users into meaningful results.
    /// No prior Star Wars knowledge required.
    /// </summary>
    public IReadOnlyList<string> ExampleQueries { get; } =
    [
        "Luke", "Vader", "Yoda", "Leia", "Han", "Obi-Wan",
        "Tatooine", "Hoth", "Coruscant", "Endor", "Dagobah",
        "Falcon", "X-wing", "Death Star", "TIE",
        "Wookiee", "Droid", "Twi'lek",
        "Hope", "Empire", "Jedi",
        "AT-AT", "Speeder", "Sand Crawler"
    ];

    // ── Constructor ──────────────────────────────────────────────────────────

    public MainViewModel(ISearchService searchService)
    {
        _searchService = searchService;
    }

    // ── Partial Hooks ────────────────────────────────────────────────────────

    /// <summary>Updates autocomplete suggestions as the user types.</summary>
    partial void OnSearchQueryChanged(string value)
    {
        FilteredSuggestions.Clear();

        if (string.IsNullOrWhiteSpace(value)) return;

        var matches = ExampleQueries
            .Where(q => q.Contains(value, StringComparison.OrdinalIgnoreCase))
            .Take(6);

        foreach (var suggestion in matches)
            FilteredSuggestions.Add(suggestion);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            ResetToWelcome();
            return;
        }

        // Cancel any in-flight search before starting a new one
        await _cts.CancelAsync();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        IsLoading    = true;
        HasResults   = false;
        HasNoResults = false;
        StatusMessage = $"Scanning the galaxy for \"{SearchQuery}\"…";
        ResultGroups.Clear();

        try
        {
            var groups = await _searchService.SearchAllAsync(SearchQuery, ct: _cts.Token);

            foreach (var group in groups)
                ResultGroups.Add(group);

            HasResults   = ResultGroups.Count > 0;
            HasNoResults = ResultGroups.Count == 0;

            StatusMessage = HasResults
                ? $"Found {ResultGroups.Sum(g => g.TotalCount)} result(s) across {ResultGroups.Count} category(ies)"
                : $"No results found for \"{SearchQuery}\" — try a different term";
        }
        catch (OperationCanceledException)
        {
            AppLogger.Instance.Debug("Search cancelled — query={Query}", SearchQuery);
        }
        catch (Exception ex)
        {
            AppLogger.Instance.Error(ex, "Search failed — query={Query}", SearchQuery);
            HasNoResults  = true;
            StatusMessage = "Unable to reach the galaxy. Check your connection and try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void UseExample(string query)
    {
        SearchQuery = query;
        _ = SearchAsync();
    }

    [RelayCommand]
    public void ClearSearch()
    {
        SearchQuery = string.Empty;
        ResetToWelcome();
    }

    // ── Private ──────────────────────────────────────────────────────────────

    private void ResetToWelcome()
    {
        ResultGroups.Clear();
        FilteredSuggestions.Clear();
        HasResults    = false;
        HasNoResults  = false;
        StatusMessage = "Type anything to search across the galaxy…";
    }
}

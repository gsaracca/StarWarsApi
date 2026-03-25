using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarWarsApi.Infrastructure;
using StarWarsApi.Services;

namespace StarWarsApi.ViewModels;

/// <summary>
/// ViewModel for the main search surface.
/// Manages the search lifecycle, dashboard metrics and reusable recent queries.
/// </summary>
public sealed partial class MainViewModel : ObservableObject
{
    private readonly ISearchService _searchService;
    private CancellationTokenSource _cts = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    [NotifyPropertyChangedFor(nameof(IsQueryEmpty))]
    [NotifyPropertyChangedFor(nameof(ActiveQueryText))]
    [NotifyPropertyChangedFor(nameof(SearchSummary))]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearSearchCommand))]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    [NotifyPropertyChangedFor(nameof(ResultStateLabel))]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearSearchCommand))]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    [NotifyPropertyChangedFor(nameof(ResultStateLabel))]
    private bool _hasResults;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowWelcome))]
    [NotifyPropertyChangedFor(nameof(ResultStateLabel))]
    private bool _hasNoResults;

    [ObservableProperty]
    private string _statusMessage = "Type a term to search the Star Wars galaxy.";

    public bool ShowWelcome => !IsLoading && !HasResults && !HasNoResults;
    public bool IsQueryEmpty => string.IsNullOrWhiteSpace(SearchQuery);
    public bool HasRecentSearches => RecentSearches.Count > 0;

    public string TotalResultsText => ResultGroups.Sum(group => group.TotalCount).ToString();
    public string CategoryCountText => ResultGroups.Count.ToString();
    public string ActiveQueryText => IsQueryEmpty ? "Waiting for input" : SearchQuery.Trim();
    public string TopCategoryText
        => ResultGroups.OrderByDescending(group => group.TotalCount).FirstOrDefault()?.Category ?? "No dominant match";

    public string ResultStateLabel
        => IsLoading ? "Scanning"
        : HasResults ? "Results ready"
        : HasNoResults ? "No matches"
        : "Idle";

    public string SearchSummary
        => IsLoading && !IsQueryEmpty ? $"Searching every category for \"{SearchQuery.Trim()}\"."
        : HasResults && !IsQueryEmpty ? $"Showing {TotalResultsText} matches for \"{SearchQuery.Trim()}\" across {CategoryCountText} categories."
        : HasNoResults && !IsQueryEmpty ? $"No matches found for \"{SearchQuery.Trim()}\"."
        : "Search once and the workspace will summarize the strongest matches here.";

    public string FooterSummary
        => HasResults ? $"{CategoryCountText} categories loaded"
        : HasRecentSearches ? $"{RecentSearches.Count} recent searches saved"
        : "Explorer ready";

    public ObservableCollection<SearchResultGroup> ResultGroups { get; } = [];
    public ObservableCollection<string> FilteredSuggestions { get; } = [];
    public ObservableCollection<string> RecentSearches { get; } = [];

    public IReadOnlyList<string> ExampleQueries { get; } =
    [
        "Luke Skywalker", "Darth Vader", "Princess Leia", "Obi-Wan Kenobi",
        "Tatooine", "Coruscant", "Dagobah", "Millennium Falcon",
        "Death Star", "X-wing", "Wookiee", "The Empire Strikes Back"
    ];

    public MainViewModel(ISearchService searchService)
    {
        _searchService = searchService;
        RecentSearches.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(HasRecentSearches));
            OnPropertyChanged(nameof(FooterSummary));
            ClearSearchCommand.NotifyCanExecuteChanged();
        };
    }

    partial void OnSearchQueryChanged(string value)
    {
        FilterSuggestions(value);
    }

    [RelayCommand(CanExecute = nameof(CanSearch))]
    public async Task SearchAsync()
    {
        var query = SearchQuery.Trim();

        if (string.IsNullOrWhiteSpace(query))
        {
            ResetToWelcome();
            return;
        }

        await _cts.CancelAsync();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        SearchQuery = query;
        IsLoading = true;
        HasResults = false;
        HasNoResults = false;
        StatusMessage = $"Scanning the galaxy for \"{query}\"...";
        ResultGroups.Clear();
        RaiseDashboardProperties();

        try
        {
            var groups = await _searchService.SearchAllAsync(query, ct: _cts.Token);

            foreach (var group in groups)
            {
                ResultGroups.Add(group);
            }

            HasResults = ResultGroups.Count > 0;
            HasNoResults = ResultGroups.Count == 0;
            TrackRecentSearch(query);

            StatusMessage = HasResults
                ? $"Found {ResultGroups.Sum(g => g.TotalCount)} matches for \"{query}\"."
                : $"No results found for \"{query}\".";
        }
        catch (OperationCanceledException)
        {
            AppLogger.Instance.Debug("Search cancelled. Query={Query}", query);
        }
        catch (Exception ex)
        {
            AppLogger.Instance.Error(ex, "Search failed. Query={Query}", query);
            HasNoResults = true;
            StatusMessage = "Unable to reach the galaxy right now. Check your connection and try again.";
        }
        finally
        {
            IsLoading = false;
            RaiseDashboardProperties();
        }
    }

    [RelayCommand]
    public void UseExample(string query)
    {
        SearchQuery = query;
        _ = SearchAsync();
    }

    [RelayCommand]
    public void UseRecentSearch(string query)
    {
        SearchQuery = query;
        _ = SearchAsync();
    }

    [RelayCommand(CanExecute = nameof(CanClearSearch))]
    public void ClearSearch()
    {
        SearchQuery = string.Empty;
        ResetToWelcome();
    }

    private bool CanSearch() => !IsLoading && !string.IsNullOrWhiteSpace(SearchQuery);

    private bool CanClearSearch() => !IsLoading && (!IsQueryEmpty || HasResults || HasNoResults || HasRecentSearches);

    private void ResetToWelcome()
    {
        ResultGroups.Clear();
        FilteredSuggestions.Clear();
        HasResults = false;
        HasNoResults = false;
        StatusMessage = "Type a term to search the Star Wars galaxy.";
        RaiseDashboardProperties();
    }

    private void FilterSuggestions(string value)
    {
        FilteredSuggestions.Clear();

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        foreach (var suggestion in ExampleQueries
                     .Where(query => query.Contains(value, StringComparison.OrdinalIgnoreCase))
                     .Take(6))
        {
            FilteredSuggestions.Add(suggestion);
        }
    }

    private void TrackRecentSearch(string query)
    {
        var existing = RecentSearches.FirstOrDefault(item => item.Equals(query, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            RecentSearches.Remove(existing);
        }

        RecentSearches.Insert(0, query);

        while (RecentSearches.Count > 6)
        {
            RecentSearches.RemoveAt(RecentSearches.Count - 1);
        }
    }

    private void RaiseDashboardProperties()
    {
        OnPropertyChanged(nameof(TotalResultsText));
        OnPropertyChanged(nameof(CategoryCountText));
        OnPropertyChanged(nameof(TopCategoryText));
        OnPropertyChanged(nameof(SearchSummary));
        OnPropertyChanged(nameof(FooterSummary));
    }
}

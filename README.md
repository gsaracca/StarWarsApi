# ★ Star Wars Explorer

> *"A long time ago in a galaxy far, far away…"*
> — but the data is very much available right now.

A professional **WinUI 3** desktop application for Windows that lets you explore the entire Star Wars universe interactively — characters, films, starships, vehicles, species and planets — powered by the public [SWAPI](https://swapi.dev) API.

![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-0078D4?logo=windows)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![WinUI](https://img.shields.io/badge/WinUI-3-0078D4?logo=microsoft)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Screenshots

| Welcome | Search results | Character cards |
|---|---|---|
| *Dark space theme with quick-search chips* | *Parallel results grouped by category* | *Images + structured data per card* |

---

## Features

- **Unified intelligent search** — one search field queries all six SWAPI endpoints simultaneously
- **Query By Example** — curated quick-search chips guide users with no prior Star Wars knowledge
- **Parallel API calls** — all six resource types are fetched concurrently with `Task.WhenAll`
- **Smart caching** — responses cached in-memory for 10 minutes; zero redundant HTTP calls
- **Character images** — real photos loaded from a GitHub-hosted image CDN, with graceful emoji fallback for other resource types
- **Structured logging** — rolling daily log file via [Serilog](https://serilog.net), stored in `%LocalAppData%\StarWarsExplorer\Logs\`
- **Star Wars dark theme** — deep-space palette with the iconic yellow accent, built as a XAML `ResourceDictionary`

---

## Tech stack

| Layer | Technology |
|---|---|
| UI Framework | WinUI 3 (Windows App SDK 1.8) |
| Target | .NET 8 — Windows 10 / 11 |
| MVVM | [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) 8.3 |
| HTTP | `HttpClient` with manual composition |
| JSON | [Newtonsoft.Json](https://www.newtonsoft.com/json) 13 |
| Caching | `Microsoft.Extensions.Caching.Memory` |
| Logging | [Serilog](https://serilog.net) + `Serilog.Sinks.File` |
| Data source | [SWAPI](https://swapi.dev) · [swapi-gallery](https://github.com/vieraboschkova/swapi-gallery) |

---

## Architecture

The solution follows a strict layered architecture with no DI container — the composition root lives entirely in `App.xaml.cs`.

```
StarWarsApi/
├── Infrastructure/
│   └── AppLogger.cs              # Serilog singleton — initialize once, use everywhere
│
├── Client/
│   ├── SwapiEndpoints.cs         # All API URL constants and builders
│   ├── ISwapiClient.cs           # Contract: one method per SWAPI resource
│   ├── SwapiClient.cs            # HTTP + transparent cache layer
│   └── VisualGuideEndpoints.cs   # Image URL resolver (characters only)
│
├── Models/                       # Pure data — mirrors the SWAPI JSON schema
│   ├── PagedResult.cs            # Generic pagination envelope { count, next, results }
│   ├── Person.cs
│   ├── Film.cs
│   ├── Starship.cs
│   ├── Vehicle.cs
│   ├── Species.cs
│   └── Planet.cs
│
├── Services/
│   ├── ICacheService.cs / CacheService.cs    # IMemoryCache wrapper
│   ├── ISearchService.cs / SearchService.cs  # Parallel search orchestrator
│
├── ViewModels/
│   ├── SearchResultItem.cs       # Flat display model — all fields pre-formatted
│   ├── SearchResultGroup.cs      # Category group + factory methods per resource type
│   └── MainViewModel.cs          # Page state, commands, suggestion filtering
│
├── Converters/                   # UI-only type converters
│   ├── HexStringToBrushConverter.cs
│   ├── BoolToVisibilityConverter.cs
│   └── StringToImageSourceConverter.cs
│
├── Themes/
│   └── StarWarsTheme.xaml        # Color tokens and brushes
│
└── Views/
    ├── MainPage.xaml             # AutoSuggestBox · chips · UniformGridLayout cards
    └── MainPage.xaml.cs          # Event handlers (search, chips, suggestions)
```

### Request lifecycle

```
User types → AutoSuggestBox filters suggestion chips
           → SearchCommand → SearchService.SearchAllAsync()
           → Task.WhenAll(6 × SwapiClient.FetchAsync())
                  └─ Cache HIT  → return cached PagedResult<T>
                  └─ Cache MISS → HTTP GET → deserialize → cache → return
           → SearchResultGroup.From*() maps models to display items
           → ItemsRepeater + UniformGridLayout renders cards
```

---

## Prerequisites

| Requirement | Version |
|---|---|
| Windows | 10 (1809+) or 11 |
| .NET SDK | 8.0+ |
| Visual Studio | 2022 v17.8+ with **Windows App SDK** workload |
| Windows App Runtime | **1.8** — install once per machine |

Install the runtime with:

```powershell
winget install Microsoft.WindowsAppRuntime.1.8
```

---

## Getting started

```bash
# 1. Clone
git clone https://github.com/your-username/StarWarsApi.git
cd StarWarsApi

# 2. Open in Visual Studio
start StarWarsApi.sln

# 3. Set platform to x64, press F5
```

> No API keys, no configuration files, no environment variables required.
> The app connects to [swapi.dev](https://swapi.dev) out of the box.

---

## How to search

The search field queries **all six resource types simultaneously**.
You don't need any Star Wars knowledge — just try:

| You type | You find |
|---|---|
| `Luke` | Luke Skywalker + related films |
| `Tatooine` | The desert planet + its residents |
| `Falcon` | Millennium Falcon specs and crew |
| `Empire` | The Empire Strikes Back full details |
| `Wookiee` | Species classification and homeworld |
| `Death` | Death Star combat specs |

Or click any of the **quick-search chips** to jump straight in.

---

## Notes on images

Character portraits are served from [`vieraboschkova/swapi-gallery`](https://github.com/vieraboschkova/swapi-gallery), a community-maintained GitHub repository with 87 images indexed by SWAPI person ID.

For films, planets, starships, vehicles and species no public CDN with SWAPI-compatible IDs currently exists — the previously popular `starwars-visualguide.com` domain expired in 2025 and was redirected to an unrelated site. These categories display an elegant emoji fallback instead.

---

## License

MIT — free to use, fork and extend.

---

> *"Do or do not. There is no try."*  — Yoda

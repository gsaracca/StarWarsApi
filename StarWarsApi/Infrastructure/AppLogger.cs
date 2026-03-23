using Serilog;
using Serilog.Core;

namespace StarWarsApi.Infrastructure;

/// <summary>
/// Centralized, structured logger backed by Serilog.
/// Initialize once at application startup; access via <see cref="Instance"/> throughout the app.
/// </summary>
public static class AppLogger
{
    private static ILogger? _instance;

    public static ILogger Instance =>
        _instance ?? throw new InvalidOperationException(
            "AppLogger has not been initialized. Call AppLogger.Initialize() before use.");

    /// <summary>
    /// Configures and starts the file-based structured logger.
    /// Rolling by day; retains the last 7 log files.
    /// </summary>
    public static void Initialize(string logFilePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);

        _instance = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("Application", "StarWarsExplorer")
            .WriteTo.File(
                path:              logFilePath,
                outputTemplate:    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{Application}] {Message:lj}{NewLine}{Exception}",
                rollingInterval:   RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared:            false)
            .CreateLogger();

        _instance.Information("=== Star Wars Explorer started === Log: {Path}", logFilePath);
    }

    /// <summary>Flushes and closes all Serilog sinks. Call on application exit.</summary>
    public static void Shutdown()
    {
        _instance?.Information("=== Star Wars Explorer shutting down ===");
        Log.CloseAndFlush();
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Config via env vars or args (optional)
        // START_NUMBER: initial value (default 0)
        // INTERVAL_SECONDS: delay between increments (default 5)
        int start = GetInt("START_NUMBER", args, 0);
        int intervalSeconds = GetInt("INTERVAL_SECONDS", args, 5);

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;    // allow graceful shutdown
            cts.Cancel();
        };

        Console.WriteLine("=== Continuous Add-One Job ===");
        Console.WriteLine($"Start: {start}, Interval: {intervalSeconds}s");
        Console.WriteLine("Press Ctrl+C to stop (locally).");

        var n = start;
        while (!cts.IsCancellationRequested)
        {
            n = checked(n + 1); // throws on overflow (so you see it in logs)
            Console.WriteLine($"{DateTime.UtcNow:O} -> {n}");
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), cts.Token);
            }
            catch (TaskCanceledException) { /* shutting down */ }
        }

        Console.WriteLine("Shutting down gracefully.");
    }

    static int GetInt(string key, string[] args, int fallback)
    {
        // env var wins
        var env = Environment.GetEnvironmentVariable(key);
        if (int.TryParse(env, out var fromEnv)) return fromEnv;

        // args like: --START_NUMBER=10 --INTERVAL_SECONDS=2
        foreach (var a in args)
        {
            var parts = a.Split('=', 2);
            if (parts.Length == 2 && parts[0].TrimStart('-', '/') == key &&
                int.TryParse(parts[1], out var v))
                return v;
        }
        return fallback;
    }
}

using System.Diagnostics;

internal static class Program
{
    private const string GameExecutable = "Allumeria.exe";
    private const string LoaderAssembly = "AlluLoader.dll";
    private const string StartupHooksVariable = "DOTNET_STARTUP_HOOKS"; 
    private static readonly StringComparison PathComparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    public static async Task<int> Main(string[] args)
    {
        string gameDirectory = AppContext.BaseDirectory; 
        string gamePath = Path.GetFullPath(Path.Combine(gameDirectory, GameExecutable));
        string hookPath = Path.GetFullPath(Path.Combine(gameDirectory, LoaderAssembly));
        if (!File.Exists(gamePath)) return ReportMissingFile("game executable", gamePath);
        if (!File.Exists(hookPath)) return ReportMissingFile("loader bootstrap", hookPath);
        var startInfo = new ProcessStartInfo
        {
            FileName = gamePath,
            WorkingDirectory = gameDirectory,
            UseShellExecute = false
        };
        AddStartupHook(startInfo, hookPath);
        // ArgumentList handles quoting and escaping each argument.
        foreach (string argument in args) startInfo.ArgumentList.Add(argument);
        try
        {
            using Process? game = Process.Start(startInfo);
            if (game is null)
            {
                Console.Error.WriteLine("The operating system did not start the game process.");
                return 1;
            }
            await game.WaitForExitAsync();
            return game.ExitCode;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"The game process failed:{Environment.NewLine}{exception}");
            return 1;
        }
    }

    private static void AddStartupHook(ProcessStartInfo startInfo, string hookPath)
    {
        startInfo.Environment.TryGetValue(StartupHooksVariable, out string? existingHooks); 
        if (string.IsNullOrEmpty(existingHooks))
        {
            startInfo.Environment[StartupHooksVariable] = hookPath;
            return;
        }  
        bool alreadyRegistered = existingHooks.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries).Any(entry => PathsEqual(entry, hookPath)); 
        if (alreadyRegistered) return;  
        string separator = existingHooks[^1] == Path.PathSeparator ? string.Empty : Path.PathSeparator.ToString(); 
        startInfo.Environment[StartupHooksVariable] = existingHooks + separator + hookPath;
    }

    private static bool PathsEqual(string candidate, string hookPath)
    {
        if (!Path.IsPathFullyQualified(candidate)) return false; 
        try
        {
            return string.Equals(Path.GetFullPath(candidate), hookPath, PathComparison);
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return false;
        }
    }

    private static int ReportMissingFile(string description, string path)
    {
        Console.Error.WriteLine($"Could not find the {description}:{Environment.NewLine}{path}"); 
        return 1;
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace AlluLoader.API.Logging
{
    /// <summary>
    /// Simple logging for mods and the API. Writes to AlluLoader/Logs/api.log.
    /// </summary>
    public static class Log
    {
        public static string LogFilePath => _logFilePath ?? "unknown";
        private static readonly object _lock = new object();
        private static string? _logFilePath;
        private static bool _initialized;

        private static void EnsureInitialized()
        {
            if (_initialized) return;

            try
            {
                var baseDir = AppContext.BaseDirectory;
                var logsDir = Path.Combine(baseDir, "AlluLoader", "Logs");
                Directory.CreateDirectory(logsDir);
                _logFilePath = Path.Combine(logsDir, "api.log");
                _initialized = true;
            }
            catch
            {
                // Fallback to current directory if we can't create the expected path
                _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "alluloader_api.log");
                _initialized = true;
            }
        }
         
        public static void Write(string message)
        {
            try
            {
                EnsureInitialized();
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath!, $"[{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz}] {message}{Environment.NewLine}");
                }
            }
            catch
            {
                // Ignore logging failures – never crash the game.
            }
        } 

        public static void Write(string message, Exception ex)
        {
            Write($"{message}{Environment.NewLine}{ex}");
        }
    }
}
using Microsoft.Extensions.Logging;
using System.IO;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    public FileLoggerProvider(string path) => _path = path;

    public ILogger CreateLogger(string categoryName) =>
        new FileLogger(_path, categoryName);

    public void Dispose() { }

    private class FileLogger : ILogger
    {
        private readonly string _path;
        private readonly string _category;
        private static readonly object _lock = new();

        public FileLogger(string path, string category)
        {
            _path = path;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
                                TState state, Exception? exception,
                                Func<TState, Exception?, string> formatter)
        {
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel,-11}] {_category,-40}: {formatter(state, exception)}";
            if (exception != null)
            {
                line += Environment.NewLine + $"    Exception: {exception}";
            }
            lock (_lock)
            {
                File.AppendAllText(_path, line + Environment.NewLine);
            }
        }
    }
}

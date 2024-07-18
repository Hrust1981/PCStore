using Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core
{
    public class CustomLogger<T> : ILogger<T>
    {
        private readonly IFileLoggerService _loggerService;
        private readonly LoggerOptions _setupOptions;

        public CustomLogger(IFileLoggerService loggerService, IOptionsMonitor<LoggerOptions> setupOptions)
        {
            _loggerService = loggerService;
            _setupOptions = setupOptions.CurrentValue;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string? PATH = _setupOptions.PathToLoggerFile;
            string message = $"[{DateTime.Now}] [{logLevel}] [{eventId}] [{typeof(T)}] - {formatter(state, exception)}";
            _loggerService.WriteToFile(PATH, message);
        }
    }
}

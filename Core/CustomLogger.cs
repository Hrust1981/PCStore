using Core.Services;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace Core
{
    public class CustomLogger<T> : ILogger<T>
    {
        private readonly IFileLoggerService _loggerService;

        public CustomLogger(IFileLoggerService loggerService)
        {
            _loggerService = loggerService;
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
            string? PATH = ConfigurationManager.AppSettings.Get("PathToLoggerFile");
            string message = $"[{DateTime.Now}] [{logLevel}] [{eventId}] [{typeof(T)}] - {formatter(state, exception)}";
            _loggerService.WriteToFile(PATH, message);
        }
    }
}

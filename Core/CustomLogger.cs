using Core.Services;
using Microsoft.Extensions.Logging;

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
            const string PATH = "C:\\Users\\Hrust\\source\\repos\\PCStore\\Logs.txt";
            string message = $"[{DateTime.Now}] [{logLevel}] [{eventId}] [{typeof(T)}] - {formatter(state, exception)}";
            _loggerService.WriteToFile(PATH, message);
        }
    }
}

namespace Core.Services
{
    public interface IFileLoggerService
    {
        async Task WriteToFileAsync(string path, string message) {}
    }
}

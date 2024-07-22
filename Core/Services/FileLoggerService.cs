
namespace Core.Services
{
    public class FileLoggerService : IFileLoggerService
    {
        public async Task WriteToFileAsync(string path, string message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path, true);
                await writer.WriteLineAsync(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

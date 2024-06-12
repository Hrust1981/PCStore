namespace Core.Services
{
    public class FileLoggerService : IFileLoggerService
    {
        public void WriteToFile(string path, string message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while writing the log to the file");
            }
        }
    }
}

namespace Core.Services
{
    public interface IFileLoggerService
    {
        public void WriteToFile(string path, string message);
    }
}

using System.Configuration;

namespace Core
{
    public static class CustomConfigurationManager
    {
        public static string GetValueByKey(string key)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return configuration.AppSettings.Settings[key].Value;
        }

        public static void SetValueByKey(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            ConfigurationManager.RefreshSection("appSettings");
            configuration.Save();
        }
    }
}

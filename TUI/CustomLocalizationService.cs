using Microsoft.Extensions.Localization;
using System.Reflection;
using TUI.Properties;

namespace Core.Services
{
    public class CustomLocalizationService : StringLocalizer<string>
    {
        private readonly IStringLocalizer _stringLocalizer;
        public CustomLocalizationService(IStringLocalizerFactory factory) : base(factory)
        {
            var assemblyName = new AssemblyName(typeof(Strings).Assembly.FullName);
            _stringLocalizer = factory.Create("Strings", assemblyName.Name);
        }

        public override LocalizedString this[string name] => _stringLocalizer[name];
    }
}

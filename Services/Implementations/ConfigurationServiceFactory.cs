using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Providers;

namespace FluentNotes.Services.Implementations
{
    public class ConfigurationServiceFactory
    {
        public static IConfigurationService CreateConfigService()
        {
            if (ApplicationTypeDetector.IsPackagedApp())
                return new PackagedConfigService();
            
            return new UnpackagedConfigService();
        }

    }
}

using FluentNotes.Services.Interfaces;
using FluentNotes.Services.Implementations;
using FluentNotes.Utils.Providers;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class AppServicesFactory
    {
        public static AppServices CreateServices()
        {
            if (ApplicationTypeDetector.IsPackagedApp())
                return new AppServices
                {
                    ConfigurationService = new PackagedConfigService(),
                    DirectoryService = new PackagedDirectoryService()
                };
            
            return new AppServices
            {
                ConfigurationService = new UnpackagedConfigService(),
                DirectoryService = new UnpackagedDirectoryService()
            };
        }

    }
}

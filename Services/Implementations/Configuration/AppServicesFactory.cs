using FluentNotes.Services.Interfaces;
using FluentNotes.Services.Implementations;
using FluentNotes.Utils.Providers;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class AppServicesFactory
    {
        public static AppServices CreateServices()
        {
            IConfigurationService configurationService;
            IDirectoryService directoryService;

            if (ApplicationTypeDetector.IsPackagedApp())
            {
                configurationService = new PackagedConfigService();
                directoryService = new PackagedDirectoryService();
            }
            else
            {
                configurationService = new UnpackagedConfigService();
                directoryService = new UnpackagedDirectoryService();
            }

            var databaseService = new DatabaseService(directoryService);

            return new AppServices
            {
                ConfigurationService = configurationService,
                DirectoryService = directoryService,
                DatabaseService = databaseService
            };
        }

    }
}

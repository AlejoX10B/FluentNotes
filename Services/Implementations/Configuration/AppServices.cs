using FluentNotes.Services.Interfaces;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class AppServices
    {
        public IConfigurationService ConfigurationService { get; set; }
        public IDirectoryService DirectoryService { get; set; }
    }
}

using System.Threading.Tasks;

namespace FluentNotes.Services.Interfaces
{
    public interface IDirectoryService
    {
        Task<string> GetAppDataDirectoryAsync();
        Task<string> GetDatabasePathAsync();
        Task InitializeDirectoriesAsync();
    }
}

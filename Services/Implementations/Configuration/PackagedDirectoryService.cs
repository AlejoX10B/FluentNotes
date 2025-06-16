using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class PackagedDirectoryService : IDirectoryService
    {
        public async Task<string> GetAppDataDirectoryAsync() =>
            ApplicationData.Current.LocalFolder.Path;

        public async Task<string> GetDatabasePathAsync()
        {
            var appDataPath = await GetAppDataDirectoryAsync();
            return Path.Combine(appDataPath, AppPaths.DatabaseFile);
        }

        public async Task InitializeDirectoriesAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            var contentFolder = await localFolder.CreateFolderAsync(AppPaths.ContentFolder, CreationCollisionOption.OpenIfExists);
            var notesFolder = await contentFolder.CreateFolderAsync(AppPaths.NotesFolder, CreationCollisionOption.OpenIfExists);
            await contentFolder.CreateFolderAsync(AppPaths.OrphanedNotesFolder, CreationCollisionOption.OpenIfExists);

            await localFolder.CreateFolderAsync(AppPaths.BackupsFolder, CreationCollisionOption.OpenIfExists);
            await localFolder.CreateFolderAsync(AppPaths.LogsFolder, CreationCollisionOption.OpenIfExists);
        }
    }
}

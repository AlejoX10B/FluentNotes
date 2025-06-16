using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class UnpackagedDirectoryService : IDirectoryService
    {
        private readonly string _appDataDirectory;

        public UnpackagedDirectoryService()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _appDataDirectory = Path.Combine(localAppData, AppPaths.AppName);
        }

        public async Task<string> GetAppDataDirectoryAsync() => _appDataDirectory;

        public async Task<string> GetDatabasePathAsync() =>
            Path.Combine(_appDataDirectory, AppPaths.DatabaseFile);

        public async Task InitializeDirectoriesAsync()
        {
            try
            {
                Directory.CreateDirectory(_appDataDirectory);

                var contentPath = Path.Combine(_appDataDirectory, AppPaths.ContentFolder);
                Directory.CreateDirectory(contentPath);

                var notesPath = Path.Combine(contentPath, AppPaths.NotesFolder);
                Directory.CreateDirectory(notesPath);

                var orphanedNotesPath = Path.Combine(contentPath, AppPaths.OrphanedNotesFolder);
                Directory.CreateDirectory(orphanedNotesPath);

                Directory.CreateDirectory(Path.Combine(_appDataDirectory, AppPaths.BackupsFolder));
                Directory.CreateDirectory(Path.Combine(_appDataDirectory, AppPaths.LogsFolder));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing directory structure: {ex.Message}");
                throw;
            }
        }
    }
}

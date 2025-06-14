using System;
using System.IO;

namespace FluentNotes.Utils
{
    public static class AppSetup
    {
        /* Rutas principales de la aplicación */

        public static string AppDataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FluentNotes");

        // App Folders
        public static string BackupsFolder => Path.Combine(AppDataFolder, "Backups");
        public static string DatabaseFolder => Path.Combine(AppDataFolder, "Database");
        public static string SettingsFolder => Path.Combine(AppDataFolder, "Settings");
        // TODO: Logger
        // public static string LogsFolder => Path.Combine(AppDataPath, "Logs");

        // Content Folders
        public static string ContentFolder => Path.Combine(AppDataFolder, "Content");
        public static string NotesFolder => Path.Combine(ContentFolder, "Notes");
        public static string OrphanedNotesFolder => Path.Combine(NotesFolder, "Orphaned");
        // TODO: Attachments
        //public static string AttachmentsFolder => Path.Combine(ContentFolder, "Attachments");

        /* App Files */

        public static string DatabasePath => Path.Combine(DatabaseFolder, "notes.db");
        public static string SettingsPath => Path.Combine(SettingsFolder, "settings.json");

        public static void Initialize()
        {
            try
            {
                CreateDirectories();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inicializando la aplicación: {ex.Message}", ex);
            }
        }

        public static bool IsFirstRun()
        {
            return !Directory.Exists(AppDataFolder) || !File.Exists(DatabasePath);
        }

        private static void CreateDirectories()
        {
            var directories = new[]
            {
                AppDataFolder,
                BackupsFolder,
                DatabaseFolder,
                SettingsFolder,
                ContentFolder,
                NotesFolder,
                OrphanedNotesFolder
            };

            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

    }
}

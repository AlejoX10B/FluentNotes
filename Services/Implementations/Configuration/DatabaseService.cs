using FluentNotes.Data;
using FluentNotes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentNotes.Services.Implementations.Configuration
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly IDirectoryService directoryService;
        private string? _connectionString;

        public DatabaseService(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;
        }

        public async Task<AppDbContext> GetDbContextAsync()
        {
            if (_connectionString == null)
            {
                var path = await directoryService.GetDatabasePathAsync();
                _connectionString = $"Data Source={path};Cache=Shared;Foreign Keys=True";
            }

            return new AppDbContext(_connectionString);
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                using var context = await GetDbContextAsync();

                await context.Database.EnsureCreatedAsync();

                await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL");
                await context.Database.ExecuteSqlRawAsync("PRAGMA synchronous=NORMAL");
                await context.Database.ExecuteSqlRawAsync("PRAGMA cache_size=1000");
                await context.Database.ExecuteSqlRawAsync("PRAGMA temp_store=MEMORY");

                System.Diagnostics.Debug.WriteLine("Base de datos inicializada correctamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inicializando la base de datos: {ex.Message}");
                throw new InvalidOperationException("No se pudo inicializar la base de datos", ex);
            }
        }

        public async Task<bool> DatabaseExistsAsync()
        {
            try
            {
                var path = await directoryService.GetDatabasePathAsync();
                return File.Exists(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error verificando la existencia de la base de datos: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetDatabaseVersionAsync()
        {
            try
            {
                using var context = await GetDbContextAsync();
                var version = await context.Database
                                            .SqlQueryRaw<string>("SELECT sqlite_version()")
                                            .FirstOrDefaultAsync();

                return version ?? "Unknown";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo la versión de la base de datos: {ex.Message}");
                return "Error";
            }
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            try
            {
                var path = await directoryService.GetDatabasePathAsync();
                if (!File.Exists(path))
                    throw new FileNotFoundException("La base de datos no existe", path);

                var backupDir = Path.GetDirectoryName(backupPath);
                if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                
                using var context = await GetDbContextAsync();
                await context.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(FULL)");

                File.Copy(path, backupPath, true);

                System.Diagnostics.Debug.WriteLine($"Backup creado exitosamente en: {backupPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error realizando el backup: {ex.Message}");
                throw new InvalidOperationException("No se pudo realizar el backup", ex);
            }
        }

        public async Task RestoreDatabaseAsync(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                    throw new FileNotFoundException("El archivo de backup no existe", backupPath);

                var path = await directoryService.GetDatabasePathAsync();
                if (File.Exists(path))
                {
                    var currentBackup = $"{path}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                    File.Copy(path, currentBackup, true);
                }

                using var context = await GetDbContextAsync();
                var tableCount = await context.Database
                                                .SqlQueryRaw<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table'")
                                                .FirstOrDefaultAsync();

                System.Diagnostics.Debug.WriteLine($"Base de datos restaurada exitosamente desde: {backupPath}");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error restaurando la base de datos: {ex.Message}");
                throw new InvalidOperationException("No se pudo restaurar la base de datos", ex);
            }
        }
    }
}

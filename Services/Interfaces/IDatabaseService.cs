using FluentNotes.Data;
using System.Threading.Tasks;

namespace FluentNotes.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<AppDbContext> GetDbContextAsync();
        Task InitializeDatabaseAsync();
        Task<bool> DatabaseExistsAsync();
        Task<string> GetDatabaseVersionAsync();
        Task BackupDatabaseAsync(string backupPath);
        Task RestoreDatabaseAsync(string backupPath);
    }
}

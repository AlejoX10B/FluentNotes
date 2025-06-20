using FluentNotes.Models;
using FluentNotes.Utils.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentNotes.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString, options =>
            {
                options.CommandTimeout(30);
            });

            #if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            #endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notebook>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Id).ValueGeneratedOnAdd();

                entity.Property(n => n.Name).IsRequired().HasMaxLength(50);
                entity.Property(n => n.Description).HasMaxLength(200);
                entity.Property(n => n.Color)
                        .HasConversion<EnumConverter<ColorPalette>>()
                        .HasDefaultValue(ColorPalette.Accent);
                entity.Property(n => n.CreatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(n => n.UpdatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(n => n.IsDeleted).HasDefaultValue(false);

                entity.HasIndex(n => n.Name);
                entity.HasIndex(n => n.IsDeleted);
                entity.HasIndex(n => n.CreatedAt);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Id).ValueGeneratedOnAdd();
                entity.Property(n => n.Title).IsRequired().HasMaxLength(100);
                entity.Property(n => n.FileType)
                        .HasConversion<EnumConverter<FileType>>()
                        .HasDefaultValue(FileType.Text);
                entity.Property(n => n.CreatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(n => n.UpdatedAt).HasDefaultValueSql("datetime('now')");
                entity.Property(n => n.IsDeleted).HasDefaultValue(false);
                
                entity.HasOne(n => n.Notebook)
                      .WithMany(nb => nb.Notes)
                      .HasForeignKey(n => n.NotebookId)
                      .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasIndex(n => n.NotebookId);
                entity.HasIndex(n => n.Title);
                entity.HasIndex(n => n.FileType);
                entity.HasIndex(n => n.IsDeleted);
                entity.HasIndex(n => n.CreatedAt);
                entity.HasIndex(n => n.UpdatedAt);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Color)
                        .HasConversion<EnumConverter<ColorPalette>>()
                        .HasDefaultValue(ColorPalette.Accent);
                entity.Property(t => t.CreatedAt).HasDefaultValueSql("datetime('now')");

                entity.HasIndex(t => t.Name).IsUnique();
                entity.HasIndex(t => t.CreatedAt);
            });

            modelBuilder.Entity<NoteTag>(entity =>
            {
                entity.HasKey(nt => new { nt.NoteId, nt.TagId });

                entity.HasOne(nt => nt.Note)
                      .WithMany(n => n.NoteTags)
                      .HasForeignKey(nt => nt.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(nt => nt.Tag)
                      .WithMany(t => t.NoteTags)
                      .HasForeignKey(nt => nt.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified &&
                        e.Entity is Notebook || e.Entity is Note);

            foreach (var entry in entries)
            {
                if (entry.Entity is Notebook notebook)
                    notebook.UpdatedAt = DateTime.Now;
                
                else if (entry.Entity is Note note)
                    note.UpdatedAt = DateTime.Now;
                
            }
        }
    }
}

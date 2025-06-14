using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Models
{
    public class Note
    {
        public string Id { get; set; } = string.Empty;
        public string NotebookId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public FileType FileType { get; set; } = FileType.Text;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string FileTypeValue => FileType.GetDescription();

        public Note() { }

        public Note(
            string notebookId,
            string title,
            string content = "",
            FileType fileType = FileType.Text)
        {
            Id = Guid.NewGuid().ToString();
            NotebookId = notebookId;
            Title = title;
            Content = content;
            FileType = fileType;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

    }
}

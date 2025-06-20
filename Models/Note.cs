using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FluentNotes.Models
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? NotebookId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public FileType FileType { get; set; } = FileType.Text;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public virtual Notebook? Notebook { get; set; }
        public virtual ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}

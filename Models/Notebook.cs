using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FluentNotes.Models
{
    public class Notebook
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; } = string.Empty;

        public ColorPalette Color { get; set; } = ColorPalette.Accent;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}

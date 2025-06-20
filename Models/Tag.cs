using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ColorPalette Color { get; set; } = ColorPalette.Accent;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}

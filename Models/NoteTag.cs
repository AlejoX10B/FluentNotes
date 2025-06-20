using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Models
{
    public class NoteTag
    {
        public Guid NoteId { get; set; }
        public Guid TagId { get; set; }

        public virtual Note Note { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}

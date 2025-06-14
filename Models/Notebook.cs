using System;

namespace FluentNotes.Models
{
    public class Notebook
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.Accent;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string ColorValue => Color.GetDescription();

        public Notebook() { }

        public Notebook(
            string name,
            string description = "",
            Color color = Color.Accent)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            Color = color;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

    }
}

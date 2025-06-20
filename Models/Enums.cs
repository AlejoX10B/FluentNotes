using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Models
{
    public enum FileType
    {
        [Description("txt")]
        Text,
        [Description("md")]
        Markdown,
    }

    public enum ColorPalette
    {
        [Description("SystemAccentColor")]
        Accent,
        [Description("SystemAccentColorLight")]
        AccentLight,
        [Description("SystemAccentColorDark")]
        AccentDark,
        [Description("#2196F3")]
        Blue,
        [Description("#4CAF50")]
        Green,
        [Description("#FF9800")]
        Orange,
        [Description("#9C27B0")]
        Purple,
        [Description("#F44336")]
        Red,
        [Description("#795548")]
        Brown,
        [Description("#607D8B")]
        BlueGrey
    }
}

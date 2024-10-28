using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class KeybindEnum
{
    public int KeybindId { get; set; }

    public string Keybind { get; set; } = null!;

    public int KeybindEnum1 { get; set; }

    public virtual DefaultKeybind? DefaultKeybind { get; set; }

    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();
}

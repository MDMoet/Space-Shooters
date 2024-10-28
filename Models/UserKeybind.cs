using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserKeybind
{
    public int UserKeybindId { get; set; }

    public int UserId { get; set; }

    public int KeybindId { get; set; }

    public int ActionId { get; set; }

    public virtual UserAction Action { get; set; } = null!;

    public virtual KeybindEnum Keybind { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

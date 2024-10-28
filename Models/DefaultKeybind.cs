using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class DefaultKeybind
{
    public int DefaultKeybindId { get; set; }

    public int ActionId { get; set; }

    public virtual UserAction Action { get; set; } = null!;

    public virtual KeybindEnum DefaultKeybindNavigation { get; set; } = null!;
}

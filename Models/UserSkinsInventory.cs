using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserSkinsInventory
{
    public int TableId { get; set; }

    public int UserId { get; set; }

    public int SkinId { get; set; }

    public sbyte Purchased { get; set; }

    public virtual Skin Skin { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

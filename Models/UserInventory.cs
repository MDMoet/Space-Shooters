using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserInventory
{
    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Level { get; set; }

    public int Amount { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

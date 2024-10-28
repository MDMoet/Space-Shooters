using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserItemInventory
{
    public int TableId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Amount { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

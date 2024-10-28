using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserEquipmentInventory
{
    public int TableId { get; set; }

    public int UserId { get; set; }

    public int EquipmentId { get; set; }

    public int Amount { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

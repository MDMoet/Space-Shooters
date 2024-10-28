using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserEquipment
{
    public int TableId { get; set; }

    public int UserId { get; set; }

    public int? EquipmentId { get; set; }

    public int Itemslot { get; set; }

    public virtual Equipment? Equipment { get; set; }

    public virtual User User { get; set; } = null!;
}

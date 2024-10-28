using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Usershop
{
    public int ShopitemId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int EquipmentId { get; set; }

    public int Worth { get; set; }

    public int Amount { get; set; }

    public sbyte Isequipment { get; set; }

    public virtual User User { get; set; } = null!;
}

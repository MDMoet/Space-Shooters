using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Itemshop
{
    public int ShopitemId { get; set; }

    public int ItemId { get; set; }

    public int EquipmentId { get; set; }

    public int Worth { get; set; }

    public sbyte Isequipment { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}

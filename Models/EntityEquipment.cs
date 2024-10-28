using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class EntityEquipment
{
    public int TableId { get; set; }

    public int EntityId { get; set; }

    public int ItemId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}

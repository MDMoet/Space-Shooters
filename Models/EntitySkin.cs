using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class EntitySkin
{
    public int EntityId { get; set; }

    public int SkinId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual Skin Skin { get; set; } = null!;
}

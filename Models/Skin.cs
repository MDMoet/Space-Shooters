using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Skin
{
    public int SkinId { get; set; }

    public string Skin1 { get; set; } = null!;

    public virtual ICollection<EntitySkin> EntitySkins { get; set; } = new List<EntitySkin>();

    public virtual ICollection<UserSkin> UserSkins { get; set; } = new List<UserSkin>();

    public virtual ICollection<UserSkinsInventory> UserSkinsInventories { get; set; } = new List<UserSkinsInventory>();
}

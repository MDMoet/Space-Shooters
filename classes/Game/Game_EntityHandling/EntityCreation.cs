using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Space_Shooters.views;
using Space_Shooters.Context;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using Space_Shooters.classes.Game.Game_DataHandling;
using Space_Shooters.Models;
using System.Security.Cryptography;

namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class DetermineEntity
    {
        internal static void GetEntityData()
        {
            int EntityId_ = WaveCheck();
            Entity EntityClass_ = new();
            EntitySkin EntitySkinClass_ = new();
            EntityStat EntityStatsClass_ = new();
            EntityEquipment EntityEquipmentClass_ = new();
            using var context = new GameContext();
            // Assuming you have a primary key 'Id' in your UserStats table
            var Entity_ = context.Entities.FirstOrDefault(ugs => ugs.EntityId == EntityId_);
            if (Entity_ != null)
            {
                EntityClass_.Name = Entity_.Name;
            }
            var EntitySkin_ = context.EntitySkins.FirstOrDefault(ugs => ugs.EntityId == EntityId_);
            if (EntitySkin_ != null)
            {
                EntitySkinClass_.Skin = context.EntitySkins
                        .Where(us => us.SkinId == EntitySkin_.SkinId)
                        .Select(us => us.Skin)
                        .FirstOrDefault();
            }
            var EntityStats_ = context.EntityStats.FirstOrDefault(ugs => ugs.EntityId == EntityId_);
            if (EntityStats_ != null)
            {
                EntityStatsClass_.MinLevel = EntityStats_.MinLevel;
                EntityStatsClass_.MaxLevel = EntityStats_.MaxLevel;
                EntityStatsClass_.Health = EntityStats_.Health;
                EntityStatsClass_.BaseDamage = EntityStats_.BaseDamage;
                EntityStatsClass_.BaseSpeed = EntityStats_.BaseSpeed;
                EntityStatsClass_.BaseAttackSpeed = EntityStats_.BaseAttackSpeed;
            }
            var EntityEquipment_ = context.EntityEquipments.Where(e => e.EntityId == EntityId_).Select(e => e.ItemId).ToArray();

            _ItemModel.ItemArray = EntityEquipment_;
            if (_EntityModel != null)
            {
                _EntityModel.Entity = EntityClass_;
                _EntityModel.EntitySkin = EntitySkinClass_;
                _EntityModel.EntityStat = EntityStatsClass_;
                _EntityModel.EntityEquipment = EntityEquipmentClass_;
            }else
            {
                _EntityModel = new()
                {
                    Entity = EntityClass_,
                    EntitySkin = EntitySkinClass_,
                    EntityStat = EntityStatsClass_,
                    EntityEquipment = EntityEquipmentClass_
                };
            }
        }
        public static int WaveCheck()
        {
            int[] ia;
            int ei_;
            Random random = new();
            using var context = new GameContext();
            // Query the database to get entities that spawn in the current wave
            var e_ = context.Entities
                                  .Where(e => e.SpawnWave <= Wave)
                                  .Select(e => e.EntityId) // Assuming 'Id' is the primary key
                                  .ToArray();
            // Assign the result to 
            ia = e_;
            if (ia.Length > 0)
            {
                return ei_ = ia[random.Next(0, e_.Length)];
            }
            else
            {
                return ei_ = ia[0];
            }
           
        }
    }
    internal class EnemyCreation
    {
        internal static void CreateEnemy()
        {
            DetermineEntity.GetEntityData();
            Random random = new();
            int l_ = random.Next(_EntityModel.EntityStat.MinLevel, _EntityModel.EntityStat.MaxLevel);
            AssignEnemy(l_);
        }
        internal static Enemy AssignEnemy(int Lvl_)
        {
            if (_Enemy != null)
            {
                _Enemy.Name = _EntityModel.Entity.Name;
                _Enemy.Level = Lvl_;
                _Enemy.Health = _EntityModel.EntityStat.Health;
                _Enemy.Base_damage = _EntityModel.EntityStat.BaseDamage;
                _Enemy.Base_speed = _EntityModel.EntityStat.BaseSpeed;
                _Enemy.Base_attack_speed = _EntityModel.EntityStat.BaseAttackSpeed;
                _Enemy.Skin = _EntityModel.EntitySkin.Skin.Skin1;
                return _Enemy;
            }
            else
            {
                _Enemy = new()
                {
                    Name = _EntityModel.Entity.Name,
                    Level = Lvl_,
                    Health = _EntityModel.EntityStat.Health,
                    Base_damage = _EntityModel.EntityStat.BaseDamage,
                    Base_speed = _EntityModel.EntityStat.BaseSpeed,
                    Base_attack_speed = _EntityModel.EntityStat.BaseAttackSpeed,
                    Skin = _EntityModel.EntitySkin.Skin.Skin1,
                    Item = null,
                    Equipment = null
                };
                return _Enemy;
            }
         
        }
    }
}

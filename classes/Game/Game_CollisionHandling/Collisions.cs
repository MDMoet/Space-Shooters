using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using System.Windows;
using System.Reflection;
using Space_Shooters.classes.Game.Game_EntityHandling;
using Space_Shooters.Models;
using Space_Shooters.classes.Game.Game_VariableHandling;
using Space_Shooters.classes.Game.Game_DataHandling;

namespace Space_Shooters.classes.Game.Game_CollisionHandling
{
    public class Collisions
    {
        internal static void CheckCollision()
        {
            var bulletsToRemove = new List<Border>();
            var entitiesToRemove = new List<Grid>();

            foreach (var bullet in BulletsList.ToList())
            {
                foreach (var enemy in EntitiesList.ToList())
                {
                    if (IsColliding(bullet, enemy))
                    {
                        HandleCollision(bullet, enemy, bulletsToRemove, entitiesToRemove);
                    }
                }
            }
            // Remove bullets and entities after iteration
           
            EntityRemoval(entitiesToRemove);
            BulletRemoval(bulletsToRemove);

        }
        internal static void EntityRemoval(List<Grid> entitiesToRemove)
        {
            foreach (var entity in entitiesToRemove)
            {
                if (_WindowModel.MainGrid.Children.Contains(entity))
                {
                    _WindowModel.MainGrid.Children.Remove(entity);
                    EntitiesLeft--;
                    EntitiesList.Remove(entity);
                    _UserModel.UserGameStat.Kills++;
                    WaveCheck.CheckForEntities();

                }
            }
        }
        internal static void BulletRemoval(List<Border> bulletsToRemove)
        {
            foreach (var bullet in bulletsToRemove)
            {
                if (_WindowModel.MainGrid.Children.Contains(bullet))
                {
                    _WindowModel.MainGrid.Children.Remove(bullet);
                    BulletsList.Remove(bullet);
                    _UserModel.UserGameStat.HitShots++;
                }
            }
        }

        private static bool IsColliding(Border bullet, Grid enemy)
        {
            // Ensure the bullet and enemy are part of the visual tree
            if (!_WindowModel.MainGrid.Children.Contains(bullet) || !_WindowModel.MainGrid.Children.Contains(enemy))
            {
                return false;
            }

            // Get the absolute position of the bullet
            var bulletTransform = bullet.TransformToAncestor(_WindowModel.MainGrid);
            var bulletPosition = bulletTransform.Transform(new Point(0, 0));
            Rect rect = new(bulletPosition.X, bulletPosition.Y, bullet.Width, bullet.Height);
            Rect bulletRect = rect;

            // Get the absolute position of the enemy
            var enemyTransform = enemy.TransformToAncestor(_WindowModel.MainGrid);
            var enemyPosition = enemyTransform.Transform(new Point(0, 0));
            Rect enemyRect = new(enemyPosition.X, enemyPosition.Y, enemy.Width, enemy.Height);

            return bulletRect.IntersectsWith(enemyRect);
        }

        private static void HandleCollision(Border bullet, Grid entity, List<Border> bulletsToRemove, List<Grid> entitiesToRemove)
        {
            foreach (ProgressBar health in entity.Children.OfType<ProgressBar>())
            {
                health.Value -= Convert.ToInt32(bullet.Tag);
               
                _UserModel.UserGameStat.DamageDone += Convert.ToInt32(bullet.Tag);
                if (health.Value <= 0)
                {
                    entitiesToRemove.Add(entity);
                    EntityRemoval(entitiesToRemove);
                    ItemHandling.GetItemData(entity);
                    LevelHandling.LevelProgression(entity);
                }
            }
            bulletsToRemove.Add(bullet);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using static Space_Shooters.classes.Game.Game_EntityHandling.DetermineEntity;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Mozilla;
using Space_Shooters.views;
using Space_Shooters.Models;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using Space_Shooters.classes.Game.Game_PlayerHandling;
using Space_Shooters.classes.Game.Game_VariableHandling;

namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class EntityWave
    {
        internal static int GetEntityWaveAmount()
        {
            Random random = new();
            _WaveModel.WaveMin += 1;
            _WaveModel.WaveMax += 2;
            int waveAmount = random.Next(_WaveModel.WaveMin, _WaveModel.WaveMax);

            return waveAmount;
        }
        public class InitiateSpawn
        {
            public InitiateSpawn() {
               
            }
            public async static void StartSpawning()
            {
                if(_WaveModel.WaveStarted)
                {
                    return;
                } 
                _WaveModel.WaveStarted = true;
                EntitiesLeft = Entity_Wave_Amount = GetEntityWaveAmount();
                for (_WaveModel.SpawnIndex = 1; _WaveModel.SpawnIndex <= Entity_Wave_Amount; _WaveModel.SpawnIndex++)
                {
                    if(_WaveModel.GameEnded)
                    {
                        _WaveModel.WaveStarted = false;
                        break;
                    }
                    //MessageBox.Show(index.ToString());
                    if (Paused)
                    {
                        await Task.Delay(200);
                        _WaveModel.SpawnIndex--; // Decrement i to retry spawning this entity.
                        continue;
                    }
                    EnemyCreation.CreateEnemy();
                    SpawnEntity();
                    await Task.Delay(Enemy_ms_Spawning); // Wait for the specified tick rate in milliseconds.
                }
                _WaveModel.WaveStarted = false;
            }
            private static void SpawnEntity()
            {
                EntitySpawning.SpawnEntityConfiguration();
            }
        }
    }
    internal static class EntitySpawning
    {
        public static void SpawnEntityConfiguration()
        {
            int sizeIncrease = 6 * 15;
            if (_Enemy.Level < 15)
            {
                sizeIncrease = 6 * _Enemy.Level;
            }

            int lifeIncrease = 6 * _Enemy.Level;
            if (_Enemy.Level <= 5)
            {
                lifeIncrease -= 10 * _Enemy.Level;
            }

            lifeIncrease = DifficultyHandling.LifeIncrease(lifeIncrease);

            Random random = new();
            int ranNum = random.Next(-728, 718);

            Grid Entity_Container = new()
            {
                Width = 31 + sizeIncrease,
                Height = 31 + sizeIncrease,
                Background = System.Windows.Media.Brushes.Transparent,
                Margin = new Thickness(ranNum, 0, 0, 430),
                Tag = $"{_Enemy.Level}"

            };

            Border Entity_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 30 + sizeIncrease,
                Height = 30 + sizeIncrease,
                Margin = new Thickness(ranNum, 0, 0, 430)
            };

            Image Entity = new()
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/img/skins/entity_skins/{_Enemy.Skin}.png", UriKind.Absolute)),
                Width = Entity_HitBox.Width - 4,
                Height = Entity_HitBox.Height - 4
            };
      

            ProgressBar Entity_HealthBar = new()
            {
                Width = Entity_HitBox.Width,
                Height = 5,
                Maximum = _Enemy.Health + lifeIncrease, 
                Value = _Enemy.Health + lifeIncrease,
                Background = System.Windows.Media.Brushes.Red,
                Foreground = System.Windows.Media.Brushes.Green,
                BorderBrush = System.Windows.Media.Brushes.Black,
                Margin = new Thickness(0, 0, 0, -29 - sizeIncrease)
            };

            Entity_Container.Children.Add(Entity);
            Entity_Container.Children.Add(Entity_HitBox);
            Entity_Container.Children.Add(Entity_HealthBar);
            _WindowModel.MainGrid.Children.Add(Entity_Container);

            EntitiesList.Add(Entity_Container);

            MoveEntity(Entity_Container, sizeIncrease);
        }

        public static async void MoveEntity(Grid Entity_Container, int addedWeight)
        {
            while (Entity_Container.Margin.Bottom > -650)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    continue;
                }
                Entity_Container.Margin = new Thickness(Entity_Container.Margin.Left, Entity_Container.Margin.Top, Entity_Container.Margin.Right, Entity_Container.Margin.Bottom - _Enemy.Base_speed);
                await Task.Delay(Enemy_ms_Movement + addedWeight);

            }

            if (_WindowModel.MainGrid.Children.Contains(Entity_Container))
            {
                _WindowModel.MainGrid.Children.Remove(Entity_Container);
                EntitiesList.Remove(Entity_Container);
                EntitiesLeft--;
                WaveCheck.CheckForEntities();
                PlayerDamageHandler.HandlePlayerDamage(_Enemy.Base_damage);
            }
        }
    }
}

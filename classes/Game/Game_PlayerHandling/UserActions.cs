using System;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System.Collections.Generic;
using Space_Shooters.Models;
using Space_Shooters.classes.Game.Game_DataHandling;

namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    interface IUserActions
    {
        void MoveLeft();
        void MoveRight();
        void Attack1();
        void Attack2();
        void Attack3();
    }
    public class UserActions : IUserActions
    {
        // Direction enum to specify movement direction
        public enum Direction
        {
            Left,
            Right
        }

        // Move method to handles both left and right movement
        private static void Move(Direction direction)
        {
            int _leftBoundary = -746;
            int _rightBoundary = 736;

            // Calculate the movement based on the direction
            double movement = direction == Direction.Left ? -_UserModel.UserStatBoosted.BaseSpeed : _UserModel.UserStatBoosted.BaseSpeed;
            // Calculate the new location based on the current margin
            double newLocation = _WindowModel.BoUser.Margin.Left + movement;

            // Clamp the new location within the boundaries
            newLocation = Math.Max(_leftBoundary, Math.Min(_rightBoundary, newLocation));

            // Apply the new margin to the user
            _WindowModel.BoUser.Margin = new Thickness(newLocation, _WindowModel.BoUser.Margin.Top, 0, 0);
        }

        public void MoveLeft()
        {
            // Move the user to the left
            Move(Direction.Left);
        }

        public void MoveRight()
        {
            // Move the user to the right
            Move(Direction.Right);
        }

        public void Attack1()
        {
            // Attack 1 logic
            Border Laser1_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(_WindowModel.BoUser.Margin.Left + 35, 0, 0, -430),
                Name = "Laser1_HitBox",
                Tag = Bullet_Damage.ToString()
            };
            Border Laser2_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 15,
                Height = 35,
                Margin = new Thickness(_WindowModel.BoUser.Margin.Left - 35, 0, 0, -430),
                Name = "Laser2_HitBox",
                Tag = Bullet_Damage.ToString()
            };
            Border Laser1 = new()
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20
            };
            Border Laser2 = new()
            {
                Background = System.Windows.Media.Brushes.Red,
                Width = 5,
                Height = 20,
            };
            Laser1_HitBox.Child = Laser1;
            Laser2_HitBox.Child = Laser2;
            _WindowModel.MainGrid.Children.Add(Laser1_HitBox);
            _WindowModel.MainGrid.Children.Add(Laser2_HitBox);

            BulletsList.Add(Laser1_HitBox);
            BulletsList.Add(Laser2_HitBox);

            MoveAttack(Laser1_HitBox, Laser2_HitBox);
        }

        private static async void MoveAttack(Border Laser1, Border Laser2)
        {
            while (Laser1.Margin.Top > -1000 && Laser2.Margin.Top > -1000)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    continue;
                }
                await Task.Delay(tickMs);
                Laser1.Margin = new Thickness(Laser1.Margin.Left, Laser1.Margin.Top - 50, Laser1.Margin.Right, Laser1.Margin.Bottom);
                Laser2.Margin = new Thickness(Laser2.Margin.Left, Laser2.Margin.Top - 50, Laser2.Margin.Right, Laser2.Margin.Bottom);
            }
            if (_WindowModel.MainGrid.Children.Contains(Laser1))
            {
                _WindowModel.MainGrid.Children.Remove(Laser1);
                BulletsList.Remove(Laser1);
                _UserModel.UserGameStat.MissedShots++;
            }
            if (_WindowModel.MainGrid.Children.Contains(Laser2))
            {
                _WindowModel.MainGrid.Children.Remove(Laser2);
                BulletsList.Remove(Laser2);
                _UserModel.UserGameStat.MissedShots++;
            }
        }

        public void Attack2()
        {
            // Attack 2 logic
        }

        public void Attack3()
        {
            // Attack 3 logic
        }
    }
}

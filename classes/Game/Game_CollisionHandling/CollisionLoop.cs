using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Space_Shooters.classes.Game.Game_CollisionHandling
{
    internal class CollisionLoop()
    {
        public static async void Start()
        {
            while (true)
            {
                if (!Paused)
                {
                    Collisions.CheckCollision();
                }
                await Task.Delay(50); // Adjust delay as needed
             }
        }
    }
}

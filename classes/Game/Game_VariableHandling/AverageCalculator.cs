using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class AverageCalculator
    {
        public static double AverageCalculation(int one, int two)
        {
            // Calculate the average of two numbers
            double solution;

            // Step 1: Add the two numbers together
            double step1 = Convert.ToDouble(one) + Convert.ToDouble(two);
            // Step 2: Divide the sum by 2
            double step2 = one / step1;

            // Step 3: Multiply the result by 100
            solution = step2 * 100;

            // Return the solution
            return solution;
        }
    }
}

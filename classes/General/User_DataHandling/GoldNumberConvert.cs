using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class GoldNumberConvert
    {
        internal static string Convert(string gold)
        {
            if (long.TryParse(gold, out long amount))
            {
                if (amount >= 1_000_000_000)
                {
                    return $"{amount / 1_000_000_000.0:F1}b"; // For billions
                }
                else if (amount >= 1_000_000)
                {
                    return $"{amount / 1_000_000.0:F1}M"; // For millions
                }
                else if (amount >= 1_000)
                {
                    return $"{amount / 1_000.0:F1}k"; // For thousands
                }
                return amount.ToString(); // Return as is if less than 1000
            }

            return gold.ToString(); // Return the original string if parsing fails
        }

    }
}

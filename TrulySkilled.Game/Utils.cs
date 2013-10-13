using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrulySkilled.Game
{
    public static class Utils
    {
        private static Random rand = new Random();

        public static bool GetRandomBool()
        {
            lock (rand)
            {
                return rand.NextDouble() < .5;
            }
        }
    }
}

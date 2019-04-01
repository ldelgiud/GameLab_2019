using Meltdown.AI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Utilities
{
    class HelperFunctions
    {
        public static double SpawnRate(Vector2 position)
        {
            if (position.Length() <= 100) return 0;
            else return 0.1;
        }

        public static double DropRate()
        {
            return 0.4f;
        }
    }
}

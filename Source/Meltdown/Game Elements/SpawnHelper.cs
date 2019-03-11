using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;

namespace Meltdown.Game_Elements
{
    class SpawnHelper
    {
        World world;

        public SpawnHelper(ref World world)
        {
            this.world = world;
        }

        public ref Entity SpawnPLayer()
        {
            //Vector2 playerPos = new Vector2(0, 900);
            throw new NotImplementedException();
        }

        public PowerPlant SpawnPlant()
        {
            throw new NotImplementedException();
        }

        public void SpawEnemies()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Components;
using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    struct PlayerInfo
    {
        public WorldTransformComponent transform;
        public int id;
        public PlayerInfo(WorldTransformComponent transform, int id)
        {
            this.transform = transform;
            this.id = id;
        }
 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Components;
using Meltdown.Graphics;
using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    struct PlayerInfo
    {
        public Transform2D transform;
        public int id;
        public PlayerInfo(Transform2D transform, int id)
        {
            this.transform = transform;
            this.id = id;
        }
 
    }
}

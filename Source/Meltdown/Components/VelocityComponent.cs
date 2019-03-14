using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Meltdown.Components
{
    class VelocityComponent
    {
        public Vector2 velocity;

        public VelocityComponent(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }
}

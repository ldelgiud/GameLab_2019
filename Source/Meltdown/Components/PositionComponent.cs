using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Nez;

namespace Meltdown.Components
{
    class PositionComponent : Component
    {
        public Vector2 position;

        public PositionComponent(Vector2 position)
        {
            this.position = position;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Meltdown.Components
{
    class PositionComponent
    {
        public Vector2 Position { get; }

        public PositionComponent(Vector2 position)
        {
            this.Position = position;
        }
    }
}

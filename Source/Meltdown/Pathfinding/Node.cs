using Meltdown.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Pathfinding
{
    class Node
    {
        bool walkable;
        Vector2 worldPosition;

        public Node(bool walkable, Vector2 worldPosition)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
        }
    }
}

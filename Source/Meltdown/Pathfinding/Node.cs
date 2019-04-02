using Meltdown.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Pathfinding
{
    class Node : IHeapItem<Node>
    {
        public bool walkable;
        Vector2 worldPosition;
        public int gridX, gridY;

        public int gCost, hCost;
        public Node parent;
        int heapIndex;

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int CompareTo(Node obj)
        {
            int compare = this.fCost.CompareTo(obj.fCost);
            if (compare == 0)
            {
                compare = this.hCost.CompareTo(obj.hCost);
            }
            return -compare;
        }
    }
}

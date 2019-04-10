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
        public bool path;
        public Vector2 WorldPosition { get; private set; }
        public int gridX, gridY;
        public int movementPenalty;

        public int gCost, hCost;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

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

        public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY, int movementPenalty)
        {
            this.walkable = walkable;
            this.WorldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
            this.movementPenalty = movementPenalty;
            this.path = false;
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

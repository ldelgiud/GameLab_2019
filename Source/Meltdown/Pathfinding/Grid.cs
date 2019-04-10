using DefaultEcs;
using Meltdown.Components;
using Meltdown.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision;

namespace Meltdown.Pathfinding
{
    class Grid
    {
        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            }
        }

        public Node[,] Nodes { get; private set; }
        float nodeRadius = Constants.NODE_RADIUS;
        Vector2 gridWorldSize = Constants.TOP_RIGHT_CORNER - Constants.BOTTOM_LEFT_CORNER;
        public int GridSizeX { get; private set; }
        public int GridSizeY { get; private set; }

        public float NodeRadius { get
            {
                return nodeRadius;
            } }

        public Grid()
        {
            float nodeDiameter = 2 * this.nodeRadius;
            this.GridSizeX = (int)Math.Round(gridWorldSize.X / nodeDiameter);
            this.GridSizeY = (int)Math.Round(gridWorldSize.Y / nodeDiameter);
            Nodes = new Node[this.GridSizeY, GridSizeX];

            for (int y = 0; y < GridSizeY; y++)
            {
                Vector2 worldPos = new Vector2(0);
                worldPos.Y = Constants.BOTTOM_BORDER + (y * nodeDiameter) + nodeRadius;
                for (int x = 0; x < GridSizeX; x++)
                {
                    worldPos.X = Constants.LEFT_BORDER + (x * nodeDiameter) + nodeRadius;
                    bool walkable = true;

                    AABB aabb = new AABB()
                    {
                        LowerBound = new Vector2(-nodeDiameter, -nodeDiameter),
                        UpperBound = new Vector2(nodeDiameter, nodeDiameter)
                    };
                    aabb.LowerBound += worldPos;
                    aabb.UpperBound += worldPos;
                    SpawnHelper.quadtree.QueryAABB((Element<Entity> collidee) =>
                    {
                        AABBComponent collideeAABB = collidee.Value.Get<AABBComponent>();
                        if (!collideeAABB.solid)
                        {
                            return true;
                        }
                        walkable = false;
                        return false;
                    }, ref aabb);

                    int movementPenalty = 0;

                    //TODO: modify movement penalty

                    Nodes[y,x] = new Node(walkable, worldPos, x, y, movementPenalty);
                }
            }
        }

        public Node NearestWalkableNeighbour(Node node)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            queue.Enqueue(node);
            visited.Add(node);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.walkable)
                {
                    return current;
                }
                foreach (Node n in this.Neighbours(current))
                {
                    if(!visited.Contains(n))
                    {
                        queue.Enqueue(n);
                        visited.Add(n);
                    }
                }
            }
            return null;
        }
        public List<Node> Neighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < this.GridSizeX && 
                        checkY >= 0 && checkY < this.GridSizeX)
                    {
                        neighbours.Add(Nodes[checkY, checkX]);
                    }
                }
            }

            return neighbours;
        }

        public Node VectorToNode(Vector2 worldPosition)
        {
            if (
                worldPosition.X >= Constants.RIGHT_BORDER &&
                worldPosition.X <= Constants.LEFT_BORDER &&
                worldPosition.Y <= Constants.BOTTOM_BORDER &&
                worldPosition.Y >= Constants.TOP_BORDER)
            {
                return null;
            }
            Vector2 relativePos = worldPosition - Constants.BOTTOM_LEFT_CORNER;
            float xPercent = relativePos.X / this.gridWorldSize.X;
            float yPercent = relativePos.Y / this.gridWorldSize.Y;
            int x = (int) MathF.Round((this.GridSizeX - 1) * xPercent);
            int y = (int) MathF.Round((this.GridSizeY - 1) * yPercent);

            return Nodes[y, x];
        }

    }
}

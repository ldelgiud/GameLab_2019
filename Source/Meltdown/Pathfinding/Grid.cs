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

        Node[,] nodes;
        float nodeRadius = Constants.NODE_RADIUS;
        Vector2 gridWorldSize = Constants.TOP_RIGHT_CORNER - Constants.BOTTOM_LEFT_CORNER;
        int gridSizeX; 
        int gridSizeY;

        public Grid()
        {
            float nodeDiameter = 2 * this.nodeRadius;
            this.gridSizeX = (int)Math.Round(gridWorldSize.X / nodeDiameter);
            this.gridSizeY = (int)Math.Round(gridWorldSize.Y / nodeDiameter);
            nodes = new Node[this.gridSizeY, gridSizeX];

            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPos = Constants.BOTTOM_LEFT_CORNER;
                worldPos.Y += (y * nodeDiameter) + nodeRadius;
                for (int x = 0; x < gridSizeX; x++)
                {
                    worldPos.X += (x * nodeDiameter) + nodeRadius;
                    bool walkable = true;

                    AABB aabb = new AABB()
                    {
                        LowerBound = new Vector2(-this.nodeRadius, -this.nodeRadius),
                        UpperBound = new Vector2(this.nodeRadius, this.nodeRadius)
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

                    nodes[y,x] = new Node(walkable, worldPos);
                }
            }
        }

        public Node VectorToNode(Vector2 worldPosition)
        {
            Debug.Assert(
                worldPosition.X <= Constants.RIGHT_BORDER &&
                worldPosition.X >= Constants.LEFT_BORDER &&
                worldPosition.Y >= Constants.BOTTOM_BORDER &&
                worldPosition.Y <= Constants.TOP_BORDER);
            Vector2 relativePos = worldPosition - Constants.BOTTOM_LEFT_CORNER;
            float xPercent = relativePos.X / this.gridWorldSize.X;
            float yPercent = relativePos.Y / this.gridWorldSize.Y;
            int x = (this.gridSizeX - 1) * this.gridSizeX;
            int y = (this.gridSizeY - 1) * this.gridSizeY;

            return nodes[y, x];
        }

    }
}

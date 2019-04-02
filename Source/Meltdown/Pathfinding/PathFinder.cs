using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
namespace Meltdown.Pathfinding
{
    class PathFinder
    {
        Grid grid;

        public PathFinder(Vector2 start, Vector2 end)
        {
            this.grid = new Grid();
            Node source = grid.VectorToNode(start);
            Node target = grid.VectorToNode(end);

            MinHeap<Node> S = new MinHeap<Node>();
            HashSet<Node> T = new HashSet<Node>();

            S.Add(source);

            while (S.Count > 0)
            {
                Node current = S.PopMin();
                T.Add(current);

                if (current == target)
                {
                    BacktrackPath(source, target);
                    return;
                }

                foreach (Node neighbour in grid.Neighbours(current))
                {
                    if (!neighbour.walkable || T.Contains(neighbour)) continue;

                    int newCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                    if(newCostToNeighbour < neighbour.gCost || !S.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, target);
                        neighbour.parent = current;

                        if (!S.Contains(neighbour)) S.Add(neighbour);
                    }
                }
            }
        }

        void BacktrackPath(Node source, Node target)
        {
            List<Node> path = new List<Node>();
            Node current = target; 
            while(current != source)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();
        }

        int GetDistance(Node source, Node target)
        {
            int dstX = Math.Abs(source.gridX - target.gridX);
            int dstY = Math.Abs(source.gridY - target.gridY);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            } else
            {
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }

    }
}

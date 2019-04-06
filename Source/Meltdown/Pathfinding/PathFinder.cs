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
        PathRequestManager requestManager; 
        Grid grid;
        public PathFinder(Grid grid, PathRequestManager requestManager)
        {
            this.grid = grid;
            this.requestManager = requestManager;
        }

        
        public IEnumerator FindPath(Vector2 start, Vector2 end)
        {
            Node source = grid.VectorToNode(start);
            Node target = grid.VectorToNode(end);
            Vector2[] wayPoints = new Vector2[0];
            bool success = false;

            if (source.walkable && target.walkable)
            {
                MinHeap<Node> S = new MinHeap<Node>();
                HashSet<Node> T = new HashSet<Node>();

                S.Add(source);

                while (S.Count > 0)
                {
                    Node current = S.PopMin();
                    T.Add(current);

                    if (current == target)
                    {
                        success = true;
                        break;
                    }

                    foreach (Node neighbour in grid.Neighbours(current))
                    {
                        if (!neighbour.walkable || T.Contains(neighbour)) continue;

                        int newCostToNeighbour = current.gCost + GetDistance(current, neighbour) + neighbour.movementPenalty;
                        if (newCostToNeighbour < neighbour.gCost || !S.Contains(neighbour))
                        {
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, target);
                            neighbour.parent = current;

                            if (!S.Contains(neighbour)) S.Add(neighbour);
                            else S.UpdateItem(neighbour);
                        }
                    }
                }
            }

            yield return null;
            if (success)
            {
                wayPoints = BacktrackPath(source, target);
            }
            requestManager.FinishedProcessingPath(wayPoints, success);
        }

        Vector2[] BacktrackPath(Node source, Node target)
        {
            List<Node> path = new List<Node>();
            Node current = target; 
            while(current != source)
            {
                path.Add(current);
                current = current.parent;
            }
            Vector2[] waypoints = SimplifyPath(path);
            waypoints.Reverse();
            return waypoints;

        }

        Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> waypoints = new List<Vector2>();
            Vector2 oldDirection = Vector2.Zero;

            for (int i = 1; i < path.Count; ++i)
            {
                Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (newDirection != oldDirection)
                {
                    waypoints.Add(path[i].WorldPosition);
                }
                oldDirection = newDirection;
            }

            return waypoints.ToArray();
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

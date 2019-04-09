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
        PathRequestManager RequestManager { get
            {
                return Game1.Instance.ActiveState.GetInstance<PathRequestManager>();
        } }

        Grid grid;
        public PathFinder(Grid grid)
        {
            this.grid = grid;
        }

        
        public void FindPath(Vector2 start, Vector2 end)
        {
            Node source = grid.VectorToNode(start);
            Node target = grid.VectorToNode(end);
            Vector2[] wayPoints = new Vector2[0];
            bool success = false;

            if (source.walkable && target.walkable)
            {
                MinHeap<Node> Open = new MinHeap<Node>();
                HashSet<Node> Closed = new HashSet<Node>();

                Open.Add(source);

                while (Open.Count > 0)
                {
                    Node current = Open.PopMin();
                    Closed.Add(current);

                    if (current == target)
                    {
                        success = true;
                        break;
                    }

                    foreach (Node neighbour in grid.Neighbours(current))
                    {
                        if (!neighbour.walkable || Closed.Contains(neighbour)) continue;

                        int newCostToNeighbour = current.gCost + GetDistance(current, neighbour) + neighbour.movementPenalty;
                        if (newCostToNeighbour < neighbour.gCost || !Open.Contains(neighbour))
                        {
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, target);
                            neighbour.parent = current;

                            if (!Open.Contains(neighbour)) Open.Add(neighbour);
                            else Open.UpdateItem(neighbour);
                        }
                    }
                }
            }

            if (success)
            {
                wayPoints = BacktrackPath(source, target);
            }
            this.RequestManager.FinishedProcessingPath(wayPoints, success);
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
            Array.Reverse(waypoints);
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
                    path[i].path = true;
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

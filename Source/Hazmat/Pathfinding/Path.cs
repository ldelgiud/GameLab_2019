using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Pathfinding
{
    class Path
    {
        //<lookpoints, turnboundaries>
        public readonly Queue<(Vector2, Line)> bounds;
        public readonly int finishLineIndex;

        public Path(Vector2[] waypoints, Vector2 start, int turnDist)
        {
            this.bounds = new Queue<(Vector2, Line)>();
            this.finishLineIndex = waypoints.Length - 1;

            Vector2 previuosPoint = start;
            for (int i = 0; i < waypoints.Length; ++i)
            {
                Vector2 currentPoint = waypoints[i];
                Vector2 dirToCurrentPoint = (currentPoint - previuosPoint);
                dirToCurrentPoint.Normalize();
                Vector2 turnBoundaryPoint = (i == finishLineIndex)? currentPoint  : currentPoint - dirToCurrentPoint * turnDist;
                Line turnboundary = new Line(turnBoundaryPoint, previuosPoint - dirToCurrentPoint*turnDist);
                previuosPoint = turnBoundaryPoint;

                this.bounds.Enqueue((waypoints[i], turnboundary));
            }
        }
    }
}

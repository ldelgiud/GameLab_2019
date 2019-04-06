using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Pathfinding
{
    class Path
    {
        public readonly Vector2[] lookPoints;
        public readonly Line[] turnBoundaries;
        public readonly int finishLineIndex;

        public Path(Vector2[] waypoints, Vector2 start, int turnDist)
        {
            this.lookPoints = waypoints;
            this.turnBoundaries = new Line[lookPoints.Length];
            this.finishLineIndex = turnBoundaries.Length - 1;

            Vector2 previuosPoint = start;
            for (int i = 0; i < lookPoints.Length; ++i)
            {
                Vector2 currentPoint = lookPoints[i];
                Vector2 dirToCurrentPoint = (currentPoint - previuosPoint);
                dirToCurrentPoint.Normalize();
                Vector2 turnBoundaryPoint = (i == finishLineIndex)? currentPoint  : currentPoint - dirToCurrentPoint * turnDist;
                this.turnBoundaries[i] = new Line(turnBoundaryPoint, previuosPoint - dirToCurrentPoint*turnDist);
                previuosPoint = turnBoundaryPoint;
            }
        }
    }
}

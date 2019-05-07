using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Utilities
{
    class Street
    {
        public List<Vector2> positions;

        public Street()
        {
            this.positions = new List<Vector2>();
        }

        public void AddTile(Vector2 position)
        {
            this.positions.Add(position);
        }

        public Vector2 FindClosestTile(Vector2 position)
        {
            if (this.positions.Count == 0) return Vector2.Zero;

            Vector2 closest = this.positions[0];
            float minSqrdDist = (position - closest).LengthSquared();
            foreach(Vector2 curr in this.positions)
            {
                float sqrdDist = (position - curr).LengthSquared();
                if (sqrdDist < minSqrdDist)
                {
                    minSqrdDist = sqrdDist;
                    closest = curr;
                }
            }
            return closest;
        }

        public int FindClosestDirection(Vector2 position)
        {
            Vector2 closest = FindClosestTile(position);
            float x = closest.X;
            float y = closest.Y;
            if (MathF.Abs(x) < MathF.Abs(y))
            {
                if (x < 0) return 2;
                else return 0;
            } else
            {
                if (y < 0) return 3;
                else return 1;
            }
        }
    }
}

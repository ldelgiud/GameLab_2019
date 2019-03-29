using Microsoft.Xna.Framework;

namespace Meltdown.Utilities.Extensions
{
    static class VectorExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector, 0);
        }
    }
}

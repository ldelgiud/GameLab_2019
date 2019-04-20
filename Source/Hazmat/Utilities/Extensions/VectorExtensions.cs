using System;

using Microsoft.Xna.Framework;

namespace Hazmat.Utilities.Extensions
{
    static class VectorExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static float ToRotation(this Vector2 vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
        }

        public static Vector2 ToVector2(this float rotation)
        {
            return new Vector2(MathF.Cos(rotation), MathF.Sin(rotation));
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector, 0);
        }

    }
}

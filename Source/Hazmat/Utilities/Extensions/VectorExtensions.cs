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

        public static Vector2 Rotate(this Vector2 vector, float radians)
        {
            float x = vector.X * MathF.Cos(radians) - vector.Y * MathF.Sin(radians);
            float y = vector.X * MathF.Sin(radians) + vector.Y * MathF.Cos(radians);

            return new Vector2(x, y);
        }

        public static Vector2 RotateAroundPoint(Vector2 Vector2rot, float radians, Vector2 origin)
        {
            return origin + Rotate(Vector2rot - origin, radians);
        }
    }
}

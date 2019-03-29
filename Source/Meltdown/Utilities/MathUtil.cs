using System;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    static class MathUtil
    {

        public static Vector4 MultiplyMatrixVector(Matrix m, Vector4 vec)
        {
            Vector4 result;
            result.X = m.M11 * vec.X + m.M12 * vec.Y + m.M13 * vec.Z + m.M14 * vec.W;
            result.Y = m.M21 * vec.X + m.M22 * vec.Y + m.M23 * vec.Z + m.M24 * vec.W;
            result.Z = m.M31 * vec.X + m.M32 * vec.Y + m.M33 * vec.Z + m.M34 * vec.W;
            result.W = m.M41 * vec.X + m.M42 * vec.Y + m.M43 * vec.Z + m.M44 * vec.W;
            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities.Extensions
{
    static class MatrixExtensions
    {
        public static float TranslationX(this Matrix matrix)
        {
            return matrix.M14;
        }

        public static float TranslationY(this Matrix matrix)
        {
            return matrix.M24;
        }

        public static float TranslationZ(this Matrix matrix)
        {
            return matrix.M34;
        }

        public static Vector3 Translation(this Matrix matrix)
        {
            return new Vector3(matrix.TranslationX(), matrix.TranslationY(), matrix.TranslationZ());
        }

        public static float RotationX(this Matrix matrix)
        {
            return MathF.Asin(-matrix.M32 / matrix.ScaleZ());
        }

        public static float RotationY(this Matrix matrix)
        {
            return MathF.Atan2(matrix.M32 / matrix.ScaleZ(), matrix.M33 / matrix.ScaleZ());
        }

        public static float RotationZ(this Matrix matrix)
        {
            return MathF.Atan2(matrix.M12 / matrix.ScaleX(), matrix.M22 / matrix.ScaleY());
        }

        public static Vector3 Rotation(this Matrix matrix)
        {
            return new Vector3(matrix.RotationX(), matrix.RotationY(), matrix.RotationZ());
        }

        public static float ScaleX(this Matrix matrix)
        {
            return MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13);
        }

        public static float ScaleY(this Matrix matrix)
        {
            return MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23);
        }

        public static float ScaleZ(this Matrix matrix)
        {
            return MathF.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33);
        }

        public static Vector3 Scale(this Matrix matrix)
        {
            return new Vector3(matrix.ScaleX(), matrix.ScaleY(), matrix.ScaleZ());
        }

    }
}

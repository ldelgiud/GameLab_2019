using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities.Extensions
{
    static class MatrixUtil
    {
        public static Matrix CreateOrthographic(float left, float right, float bottom, float top, float near, float far)
        {
            return new Matrix(
                2.0f / (right - left), 0, 0, -(right + left) / (right - left),
                0, 2 / (top - bottom), 0, -(top + bottom) / (top - bottom),
                0, 0, -2 * (far - near), -(far + near) / (far - near),
                0, 0, 0, 1
                );
        }
    }

    static class MatrixExtensions
    {
        public static float TranslationX(this ref Matrix matrix)
        {
            return matrix.M14;
        }

        public static float TranslationY(this ref Matrix matrix)
        {
            return matrix.M24;
        }

        public static float TranslationZ(this ref Matrix matrix)
        {
            return matrix.M34;
        }

        public static Vector3 Translation(this ref Matrix matrix)
        {
            return new Vector3(matrix.TranslationX(), matrix.TranslationY(), matrix.TranslationZ());
        }

        public static Matrix NegateTranslation(this Matrix matrix)
        {
            matrix.M14 *= -1;
            matrix.M24 *= -1;
            matrix.M34 *= -1;
            return matrix;
        }

        public static float RotationX(this ref Matrix matrix)
        {
            return MathF.Asin(-matrix.M23 / matrix.ScaleZ());
        }

        public static float RotationY(this ref Matrix matrix)
        {
            return MathF.Atan2(matrix.M13 / matrix.ScaleZ(), matrix.M33 / matrix.ScaleZ());
        }

        public static float RotationZ(this ref Matrix matrix)
        {
            return MathF.Atan2(matrix.M21 / matrix.ScaleX(), matrix.M22 / matrix.ScaleY());
        }

        public static Vector3 Rotation(this ref Matrix matrix)
        {
            return new Vector3(matrix.RotationX(), matrix.RotationY(), matrix.RotationZ());
        }

        public static float ScaleX(this ref Matrix matrix)
        {
            return MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M21 * matrix.M21 + matrix.M31 * matrix.M31);
        }

        public static float ScaleY(this ref Matrix matrix)
        {
            return MathF.Sqrt(matrix.M12 * matrix.M12 + matrix.M22 * matrix.M22 + matrix.M32 * matrix.M32);
        }

        public static float ScaleZ(this ref Matrix matrix)
        {
            return MathF.Sqrt(matrix.M13 * matrix.M13 + matrix.M23 * matrix.M23 + matrix.M33 * matrix.M33);
        }

        public static Vector3 Scale(this ref Matrix matrix)
        {
            return new Vector3(matrix.ScaleX(), matrix.ScaleY(), matrix.ScaleZ());
        }

        public static String Debug(this Matrix matrix)
        {
            return String.Format("[ {0} {1} {2} {3} ]\n[ {4} {5} {6} {7} ]\n[ {8} {9} {10} {11} ]\n[ {12} {13} {14} {15} ]",
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44);
        }

    }
}

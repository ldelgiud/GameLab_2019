using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace Meltdown.Graphics
{
    struct Matrix3
    {
        Matrix matrix;

        public Matrix3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
        {
            this.matrix = new Matrix(m11, m12, m13, 0, m21, m22, m23, 0, m31, m32, m33, 0, 0, 0, 0, 0);
        }

        public static Matrix3 Identity
        {
            get
            {
                return new Matrix3() { matrix = new Matrix() { M11 = 1, M22 = 1, M33 = 1 } };
            }
        }

        public static Matrix3 CreateTranslation(Vector2 translation)
        {
            return new Matrix3() { matrix = new Matrix() { M11 = 1, M22 = 1, M31 = translation.X, M32 = translation.Y, M33 = 1 } };
        }

        public static Matrix3 CreateRotation(float radians)
        {
            return new Matrix3() { matrix = new Matrix() { M11 = MathF.Cos(radians), M12 = MathF.Sin(radians), M21 = -MathF.Sin(radians), M22 = MathF.Cos(radians), M33 = 1 } };
        }

        public static Matrix3 CreateScaling(Vector2 scale)
        {
            return new Matrix3() { matrix = new Matrix() { M11 = scale.X, M22 = scale.Y, M33 = 1 } };
        }

        public static Matrix3 operator -(Matrix3 a)
        {
            return new Matrix3() { matrix = -a.matrix };
        }

        public static Matrix3 operator +(Matrix3 a, Matrix3 b)
        {
            return new Matrix3() { matrix = a.matrix + b.matrix };
        }

        public static Matrix3 operator -(Matrix3 a, Matrix3 b)
        {
            return new Matrix3() { matrix = a.matrix - b.matrix };
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            return new Matrix3() { matrix = a.matrix * b.matrix };
        }

        public float TranslationX()
        {
            return this.matrix.M31;
        }

        public float TranslationY()
        {
            return this.matrix.M32;
        }

        public Vector2 Translation()
        {
            return new Vector2(this.matrix.M31, this.matrix.M32);
        }

        public float Rotation()
        {
            return MathF.Atan2(this.matrix.M12 / this.ScalingX(), this.matrix.M22 / this.ScalingY());
        }

        public float ScalingX()
        {
            return MathF.Sqrt(this.matrix.M11 * this.matrix.M11 + this.matrix.M12 * this.matrix.M12);
        }

        public float ScalingY()
        {
            return MathF.Sqrt(this.matrix.M21 * this.matrix.M21 + this.matrix.M22 * this.matrix.M22);
        }

        public Vector2 Scaling()
        {
            return new Vector2(this.ScalingX(), this.ScalingY());
        }

        public override string ToString()
        {
            return String.Format(
                "[ {0} {1} {2} ]\n" +
                "[ {3} {4} {5} ]\n" +
                "[ {6} {7} {8} ]",
                this.matrix.M11,
                this.matrix.M12,
                this.matrix.M13,
                this.matrix.M21,
                this.matrix.M22,
                this.matrix.M23,
                this.matrix.M31,
                this.matrix.M32,
                this.matrix.M33
                );
        }
    }
}

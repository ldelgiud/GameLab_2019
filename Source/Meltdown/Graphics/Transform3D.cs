using System;

using Microsoft.Xna.Framework;

using Meltdown.Utilities.Extensions;

namespace Meltdown.Graphics
{
    class Transform3D
    {
        Transform3D parent;
        public Transform3D Parent { get { return this.parent; } set { this.dirty = true; this.parent = value; } }

        bool dirty = true;

        Vector3 translation;
        public Vector3 LocalTranslation { get { return this.translation; } set { this.dirty = true; this.translation = value; } }

        Vector3 rotation;
        public Vector3 LocalRotation { get { return this.rotation; } set { this.dirty = true; this.rotation = value; } }

        Vector3 scale;
        public Vector3 LocalScale { get { return this.scale; } set { this.dirty = true; this.scale = value; } }

        Matrix transform;
        public Matrix LocalTransformMatrix
        {
            get
            {
                if (this.dirty)
                {
                    this.transform =
                        Matrix.CreateScale(this.LocalScale) *
                        Matrix.CreateRotationX(this.LocalRotation.X) *
                        Matrix.CreateRotationY(this.LocalRotation.Y) *
                        Matrix.CreateRotationZ(this.LocalRotation.Z) *
                        Matrix.CreateTranslation(this.LocalTranslation);
                }

                return this.transform;
            }
        }

        public Matrix LocalTranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(this.translation);
            }
        }

        public Matrix LocalRotationXMatrix
        {
            get
            {
                return Matrix.CreateRotationX(this.rotation.X);
            }
        }

        public Matrix LocalRotationYMatrix
        {
            get
            {
                return Matrix.CreateRotationY(this.rotation.Y);
            }
        }

        public Matrix LocalRotationZMatrix
        {
            get
            {
                return Matrix.CreateRotationZ(this.rotation.Z);
            }
        }

        public Matrix LocalScalingMatrix
        {
            get
            {
                return Matrix.CreateScale(this.scale);
            }
        }


        public Matrix TransformMatrix
        {
            get
            {
                return this.parent == null ? this.LocalTransformMatrix : this.LocalTransformMatrix * this.parent.TransformMatrix;
            }
        }

        public float TranslationX
        {
            get
            {
                return this.TransformMatrix.TranslationX();
            }
        }

        public float TranslationY
        {
            get
            {
                return this.TransformMatrix.TranslationY();
            }
        }

        public float TranslationZ
        {
            get
            {
                return this.TransformMatrix.TranslationZ();
            }
        }

        public Vector3 Translation
        {
            get
            {
                return this.TransformMatrix.Translation();
            }
        }

        public float RotationX
        {
            get
            {
                return this.TransformMatrix.RotationX();
            }
        }

        public float RotationY
        {
            get
            {
                return this.TransformMatrix.RotationY();
            }
        }

        public float RotationZ
        {
            get
            {
                return this.TransformMatrix.RotationZ();
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return this.TransformMatrix.Rotation();
            }
        }

        public float ScaleX
        {
            get
            {
                return this.TransformMatrix.ScaleX();
            }
        }

        public float ScaleY
        {
            get
            {
                return this.TransformMatrix.ScaleY();
            }
        }

        public float ScaleZ
        {
            get
            {
                return this.TransformMatrix.ScaleZ();
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.TransformMatrix.Scale();
            }
        }

        public Transform3D(Vector3? position = null, Vector3? rotation = null, Vector3? scale = null, Transform3D parent = null)
        {
            this.translation = position ?? Vector3.Zero;
            this.rotation = rotation ?? Vector3.Zero;
            this.scale = scale ?? Vector3.One;
            this.parent = parent;
        }
    }
}

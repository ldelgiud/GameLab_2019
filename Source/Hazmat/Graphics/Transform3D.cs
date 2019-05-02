using System;

using Microsoft.Xna.Framework;

using Hazmat.Utilities.Extensions;

namespace Hazmat.Graphics
{
    class Transform3D
    {
        public Transform3D Parent { get; set; }

        bool LocalDirty { get; set; }
        bool GlobalDirty { get { return this.LocalDirty || (this.Parent != null && this.Parent.GlobalDirty); } }

        Vector3 translation;
        public Vector3 LocalTranslation { get { return this.translation; } set { this.LocalDirty = true; this.translation = value; } }

        Vector3 rotation;
        public Vector3 LocalRotation { get { return this.rotation; } set { this.LocalDirty = true; this.rotation = value; } }

        Vector3 scale;
        public Vector3 LocalScale { get { return this.scale; } set { this.LocalDirty = true; this.scale = value; } }

        Matrix localTransform;
        public Matrix LocalTransformMatrix
        {
            get
            {
                if (this.LocalDirty)
                {
                    this.localTransform =
                        Matrix.CreateScale(this.LocalScale) *
                        Matrix.CreateRotationZ(this.LocalRotation.Z) *
                        Matrix.CreateRotationY(this.LocalRotation.Y) *
                        Matrix.CreateRotationX(this.LocalRotation.X) *
                        Matrix.CreateTranslation(this.LocalTranslation);

                    this.LocalDirty = false;
                }

                return this.localTransform;
            }
        }

        public Matrix TransformMatrix
        {
            get
            {
                if (this.Parent == null)
                {
                    return this.LocalTransformMatrix;
                }
                else
                {
                    return this.LocalTransformMatrix * this.Parent.TransformMatrix;
                }
            }
        }

        public Vector3 Translation
        {
            get
            {
                return this.TransformMatrix.Translation();
            }

            set
            {
                this.Translation = this.Parent == null ? value : value - this.Parent.TransformMatrix.Translation();
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return this.Parent == null ? this.LocalRotation : this.LocalRotation + this.Parent.Rotation;
            }

            set
            {
                this.LocalRotation = this.Parent == null ? value : value - this.Parent.Rotation;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.Parent == null ? this.LocalScale : this.LocalScale * this.Parent.Scale;
            }
        }

        public Transform3D(Vector3? position = null, Vector3? rotation = null, Vector3? scale = null, Transform3D parent = null)
        {
            this.LocalTranslation = position ?? Vector3.Zero;
            this.LocalRotation = rotation ?? Vector3.Zero;
            this.LocalScale = scale ?? Vector3.One;
            this.Parent = parent;
        }
    }
}

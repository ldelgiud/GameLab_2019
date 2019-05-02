using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hazmat.Graphics
{
    class Transform2D
    {
        public Transform2D Parent { get; set; }

        bool LocalDirty { get; set; }
        bool GlobalDirty { get { return this.LocalDirty || (this.Parent != null && this.Parent.GlobalDirty); } }

        Vector2 translation;
        public Vector2 LocalTranslation { get { return this.translation; } set { this.LocalDirty = true;  this.translation = value; } }

        float rotation;
        public float LocalRotation { get { return this.rotation; } set { this.LocalDirty = true; this.rotation = value; } }

        Vector2 scale;
        public Vector2 LocalScale { get { return this.scale; } set { this.LocalDirty = true; this.scale = value; } }

        Matrix3 localTransform;
        public Matrix3 LocalTransformMatrix
        {
            get
            {
                if (this.LocalDirty)
                {
                    this.localTransform =
                        Matrix3.CreateScaling(this.LocalScale) *
                        Matrix3.CreateRotation(this.LocalRotation) *
                        Matrix3.CreateTranslation(this.LocalTranslation);

                    this.LocalDirty = false;
                }
                return this.localTransform;
            }
        }

        public Matrix3 TransformMatrix
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

        public Vector2 Translation
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

        public float Rotation
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

        public Vector2 Scale
        {
            get
            {
                return this.Parent == null ? this.LocalScale : this.LocalScale * this.Parent.Scale;
            }

            set
            {
                this.LocalScale = this.Parent == null ? value : scale / this.Parent.Scale;
            }
        }

        public Transform2D(Vector2? position = null, float? rotation = null, Vector2? scale = null, Transform2D parent = null)
        {
            this.LocalTranslation = position ?? Vector2.Zero;
            this.LocalRotation = rotation ?? 0f;
            this.LocalScale = scale ?? Vector2.One;
            this.Parent = parent;
        }
    }

}
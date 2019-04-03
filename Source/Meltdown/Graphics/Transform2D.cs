using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Graphics
{
    class Transform2D
    {
        Transform2D parent;
        public Transform2D Parent { get { return this.parent; } set { this.dirty = true; this.parent = value; } }

        bool dirty = true;

        Vector2 translation;
        public Vector2 LocalTranslation { get { return this.translation; } set { this.dirty = true;  this.translation = value; } }

        float rotation;
        public float LocalRotation { get { return this.rotation; } set { this.dirty = true; this.rotation = value; } }

        Vector2 scale;
        public Vector2 LocalScale { get { return this.scale; } set { this.dirty = true; this.scale = value; } }

        Matrix3 localTransform;
        public Matrix3 LocalTransformMatrix
        {
            get
            {
                if (this.dirty)
                {
                    this.localTransform =
                        Matrix3.CreateScaling(this.LocalScale) *
                        Matrix3.CreateRotation(this.LocalRotation) *
                        Matrix3.CreateTranslation(this.LocalTranslation);

                    this.dirty = false;
                }
                return this.localTransform;
            }
        }

        public Matrix3 LocalTranslationMatrix
        {
            get
            {
                return Matrix3.CreateTranslation(this.LocalTranslation);
            }
        }

        public Matrix3 LocalRotationMatrix
        {
            get
            {
                return Matrix3.CreateRotation(this.LocalRotation);
            }
        }

        public Matrix3 LocalScaleMatrix
        {
            get
            {
                return Matrix3.CreateScaling(this.LocalScale);
            }
        }

        public Matrix3 TransformMatrix
        {
            get
            {
                return this.parent == null ? this.LocalTransformMatrix : this.LocalTransformMatrix * this.Parent.TransformMatrix;
            }
        }

        public Vector2 Translation
        {
            get
            {
                return this.TransformMatrix.Translation();
            }
        }

        public float Rotation
        {
            get
            {
                return this.TransformMatrix.Rotation();
            }
        }

        public Vector2 Scale
        {
            get
            {
                return this.TransformMatrix.Scaling();
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
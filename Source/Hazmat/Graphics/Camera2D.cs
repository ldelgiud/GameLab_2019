using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using Hazmat.Utilities;

namespace Hazmat.Graphics
{
    class Camera2D
    {
        // Convert to a perspective view of 30deg
        public static Vector2 WorldToPerspective(Vector2 position)
        {
            return new Vector2(
                (MathF.Sqrt(2f) / 2f) * (position.X - position.Y),
                (MathF.Sqrt(6f) / 6f) * (position.X + position.Y)
                );
        }

        // Convert from a perspective view of 30deg
        public static Vector2 PerspectiveToWorld(Vector2 position)
        {
            return new Vector2(
                (MathF.Sqrt(2f) / 2f) * position.X + (MathF.Sqrt(6f) / 2f) * position.Y,
                (MathF.Sqrt(6f) / 2f) * position.Y - (MathF.Sqrt(2f) / 2f) * position.X
                );
        }

        bool perspective;

        public Transform2D Transform { get; set; }

        public float WindowWidth { get; private set; }
        public float WindowHeight { get; private set; }
        float WindowAspectRatio { get { return this.WindowWidth / this.WindowHeight; } }

        public float ViewportWidth { get; set; }
        public float ViewportHeight { get; set; }

        public float ViewportAspectRatio { get { return this.ViewportWidth / this.ViewportHeight; } }

        public float AspectRatioRatio { get { return this.ViewportAspectRatio / this.WindowAspectRatio; } }
        public float WidthRatio { get { return this.ViewportWidth / this.WindowWidth; } }
        public float HeightRatio { get { return this.ViewportHeight / this.WindowHeight; } }

        public float ScreenWidth
        {
            get
            {
                return MathF.Max(this.ViewportHeight * (this.WindowWidth / this.WindowHeight), this.ViewportWidth);
            }
        }

        public float ScreenHeight
        {
            get
            {
                return MathF.Max(this.ViewportWidth * (this.WindowHeight / this.WindowWidth), this.ViewportHeight);
            }
        }

        float SafeZoneWidth
        {
            get
            {
                return MathF.Min(this.WindowHeight * (this.ViewportWidth / this.ViewportHeight), this.WindowWidth);
            }
        }

        float SafeZoneHeight
        {
            get
            {
                return MathF.Min(this.WindowWidth * (this.ViewportHeight / this.ViewportWidth), this.WindowHeight);
            }
        }

        Vector2 SafeZoneOffset
        {
            get
            {
                return new Vector2(
                    (this.WindowWidth - this.SafeZoneWidth) / 2,
                    (this.WindowHeight - this.SafeZoneHeight) / 2
                    );
            }
        }

        public Matrix3 View
        {
            get
            {
                if (this.perspective)
                {
                    return Matrix3.CreateTranslation(-Camera2D.WorldToPerspective(this.Transform.Translation)) *
                        Matrix3.CreateRotation(-this.Transform.Rotation) *
                        Matrix3.CreateScaling(Vector2.One / this.Transform.Scale);
                }
                else
                {
                    return Matrix3.CreateTranslation(-this.Transform.Translation) *
                        Matrix3.CreateRotation(-this.Transform.Rotation) *
                        Matrix3.CreateScaling(Vector2.One / this.Transform.Scale);
                }
            }
        }

        public Matrix3 Projection
        {
            get
            {
                return Matrix3.CreateScaling(new Vector2(this.SafeZoneWidth, this.SafeZoneHeight)) *
                    Matrix3.CreateTranslation(new Vector2(this.SafeZoneWidth / 2, this.SafeZoneHeight / 2));
            }
        }



        public Camera2D(Transform2D transform, float viewportWidth, float viewportHeight, bool perspective = false)
        {
            this.Transform = transform;
            this.ViewportWidth = viewportWidth;
            this.ViewportHeight = viewportHeight;
            this.perspective = perspective;

            this.Transform.LocalScale = new Vector2(this.ViewportWidth, this.ViewportHeight);
        }

        public (Vector2, float, Vector2) ToScreenCoordinates(Transform2D transform, Texture2DInfo info)
        {
            var model = transform.TransformMatrix;
            if (this.perspective)
            {
                model.ApplyWorldToPerspective();
            }
            var view = this.View;
            var projection = this.Projection;

            var mvp = model * view * projection;

            var position = this.SafeZoneOffset + new Vector2(0, this.SafeZoneHeight) + (mvp.Translation() + info.translation) * new Vector2(1, -1); // Normalize to screen coordinates
            var rotation = -(mvp.Rotation() + info.rotation);
            var scale = mvp.Scaling() * info.scale;

            return (
                position,
                rotation,
                scale
                );
        }

        public void Update(GameWindow window)
        {
            this.WindowWidth = window.ClientBounds.Width;
            this.WindowHeight = window.ClientBounds.Height;
        }

    }
}

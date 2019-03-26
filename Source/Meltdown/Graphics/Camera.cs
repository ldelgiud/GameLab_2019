using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Graphics
{
    class Camera
    {
        public Transform Transform { get; set; }
        public Matrix Projection { get; private set; }

        public Matrix View
        {
            get
            {
                return Matrix.Transpose(Matrix.CreateTranslation(-this.Transform.position));
            }
        }


        public Camera(Transform transform, Matrix projection)
        {
            this.Transform = transform;
            this.Projection = projection;
        }

        public Vector2 ToScreenCoordinates(Rectangle screen, Matrix mvp, Rectangle bounds)
        {
            Vector2 center = (Vector2.One + mvp.Translation().ToVector2()) * 0.5f * // Normalize to [0, 1]
                screen.Size.ToVector2(); // Scale to screen size
            Matrix rotation = Matrix.Transpose(Matrix.CreateRotationZ(mvp.RotationZ())) * Matrix.Transpose(Matrix.CreateTranslation(mvp.Scale() * new Vector3(bounds.Size.ToVector2(), 0) * new Vector3(-bounds.Width / 2, -bounds.Height / 2, 0)));

            return center + rotation.Translation().ToVector2();
        }
    }
}

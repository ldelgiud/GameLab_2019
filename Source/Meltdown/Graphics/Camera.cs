﻿using System;
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

        public (Vector2, float, Vector2, Vector2) ToScreenCoordinates(Rectangle screen, Matrix model, Rectangle bounds)
        {
            var mvp = this.Projection * this.View * model;

            return (
                // position
                (Vector2.One + mvp.Translation().ToVector2() * new Vector2(1, -1)) * 0.5f * screen.Size.ToVector2(),
                // rotation
                mvp.RotationZ(),
                // scale
                mvp.Scale().ToVector2() * screen.Size.ToVector2() / 2,
                // origin
                bounds.Size.ToVector2() / 2
            );
        }
    }
}
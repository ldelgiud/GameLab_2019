using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Utilities
{
    class Camera
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public Rectangle View { get { return new Rectangle(); } }

        public float AspectRatio
        {
            get
            {
                return Width / Height;
            }
        }

        public Camera(GraphicsDevice graphicsDevice, int width, int height, float x, float y)
        {
            this.Width = width;
            this.Height = height;
            this.X = x;
            this.Y = y;
        }



    }
}

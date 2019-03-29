using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Utilities
{
    public class Screen
    {
        GameWindow window;

        public int Width { get; }
        public int Height { get; }

        float WidthRatio { get { return (float)this.window.ClientBounds.Width / (float)this.Width; } }
        float HeightRatio { get { return (float)this.window.ClientBounds.Height / (float)this.Height; } }

        public float AspectRatio
        {
            get
            {
                return this.Width / this.Height;
            }
        }

        public Screen(GameWindow window, int width, int height)
        {
            this.window = window;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle Project(Vector3 translation, Vector3 scaling, Rectangle bounds)
        {
            // Bounds mapped to graphics device, adjust for center positioning
            return new Rectangle(
                (int)((translation.X - bounds.Width * scaling.X / 2) * this.WidthRatio),
                (int)((translation.Y - bounds.Height * scaling.Y / 2) * this.HeightRatio),
                (int)(bounds.Width * scaling.X * this.WidthRatio),
                (int)(bounds.Height * scaling.Y * this.HeightRatio)
                );
        }
    }
}

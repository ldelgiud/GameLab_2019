
using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    public class Camera
    {
        GameWindow window;

        public BoundingBox Bounds { get; set; }
        public Vector2 Translation { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public float AspectRatio { get { return (float)this.Width / (float)this.Height; } }
        
        public float WidthRatio { get { return (float)this.window.ClientBounds.Width / (float)this.Width; } }
        public float HeightRatio { get { return (float)this.window.ClientBounds.Height / (float)this.Height; } }

        public Camera(GameWindow window, int width, int height)
        {
            this.window = window;
            this.Width = width;
            this.Height = height;

            this.Translation = new Vector2(0f, 0f);

            this.Bounds = new BoundingBox(
                new Vector3(-(float)width / 2, -(float)height / 2, 0),
                new Vector3((float)width / 2, (float)height / 2, 0)
                );
        }

        public bool Intersects(Vector2 translation, BoundingBox box)
        {
            Vector2 relativeTranslation = translation - this.Translation;

            BoundingBox relativeBounds = new BoundingBox(
                box.Min + new Vector3(relativeTranslation, 0),
                box.Max + new Vector3(relativeTranslation, 0)
                );
            return this.Bounds.Intersects(relativeBounds);
        }

        public Rectangle Project(Vector2 translation, Vector2 Scale, Rectangle bounds)
        {
            Vector2 relativeTranslation = translation - this.Translation;

            return new Rectangle(
                (int)((relativeTranslation.X - (bounds.Width / 2) * Scale.X) * this.WidthRatio) + window.ClientBounds.Width / 2,
                (int)((-relativeTranslation.Y - (bounds.Height / 2) * Scale.X) * this.HeightRatio) + window.ClientBounds.Height / 2,
                (int)(bounds.Width * this.WidthRatio * Scale.X),
                (int)(bounds.Height * this.HeightRatio * Scale.Y)
                );
        }


        // Used for Gun -> temporary -> make direction work (mouse point projection)
        public Point ProjectPoint(Vector2 point)
        {
            Vector2 relativeTranslation = point - this.Translation;

            return new Point(
                (int)((relativeTranslation.X)) - window.ClientBounds.Width / 2,
                (int)((-relativeTranslation.Y)) + window.ClientBounds.Height / 2
                );
        }

        public Point InverseProjectPoint(Vector2 point)
        {
            Vector2 relativeTranslation = point - this.Translation;

            return new Point(
                (int)((relativeTranslation.X)) + window.ClientBounds.Width / 2,
                (int)((-relativeTranslation.Y)) + window.ClientBounds.Height / 2
                );
        }
    }
}

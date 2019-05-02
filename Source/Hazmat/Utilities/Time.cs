using System;

using Microsoft.Xna.Framework;

namespace Hazmat.Utilities
{
    public class Time
    {
        public float Absolute { get; private set; }
        public float Delta { get; private set; }
        public TimeSpan AbsoluteSpan { get; private set; }
        public TimeSpan DeltaSpan { get; private set; }


        public Time()
        {
            this.Delta = 0.0f;
            this.Absolute = 0.0f;
            this.AbsoluteSpan = new TimeSpan();
            this.DeltaSpan = new TimeSpan();
        }

        public void Update(GameTime gameTime)
        {
            this.Absolute = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f;
            this.Delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            this.AbsoluteSpan = gameTime.TotalGameTime;
            this.DeltaSpan = gameTime.ElapsedGameTime;
        }
    }
}

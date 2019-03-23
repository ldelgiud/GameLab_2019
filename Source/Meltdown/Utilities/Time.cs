using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    public class Time
    {
        public float Absolute { get; private set; }
        public float Delta { get; private set; }


        public Time()
        {
            this.Delta = 0.0f;
        }

        public void Update(GameTime gameTime)
        {
            this.Absolute = (float)gameTime.TotalGameTime.TotalMilliseconds;
            this.Delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
        }
    }
}

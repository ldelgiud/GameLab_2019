//XNA Includes
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//System Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Meltdown
{
    class PowerPlant
    {
        public Vector2 Position { get; }
        public Texture2D texture;
        public const int R = 900;
        /// <summary>
        ///  Generates new PowerPlant at distance R from (0,0) i.e. assumed players position
        ///  and random angle between 0 and pi/2
        /// </summary>
        /// <param name="texture">Texture to assign to Powerplant</param>
        public PowerPlant(Texture2D texture)
        {
            Random random = new Random();
            double angle = random.NextDouble() * Math.PI / 2.0;
            
            double x = R * Math.Cos(angle);
            //TODO: change this once camera work is done
            double y = R * Math.Sin(angle);
            this.Position = new Vector2((float)x, (float)y);

            this.texture = texture;
        }

        /// <summary>
        /// Debugging version for generating the power plant at the desired position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="texture"></param>
        public PowerPlant(Vector2 pos, Texture2D texture)
        {
            this.Position = pos;
            this.texture = texture;
        }
    }
}

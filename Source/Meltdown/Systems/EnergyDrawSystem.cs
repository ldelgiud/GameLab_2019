using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Systems
{
    class EnergyDrawSystem : ISystem<Time>, IDisposable
    {

        public bool IsEnabled { get; set; } = true;

        Energy energy;
        Texture2D texture;
        SpriteBatch spriteBatch;
        private SpriteFont font;

        public EnergyDrawSystem(Energy energy, Texture2D texture, GraphicsDevice graphicsDevice, SpriteFont font)
        {
            this.energy = energy;
            this.texture = texture;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.font = font;
        }

        public void Update(Time gameTime)
        {
            this.spriteBatch.Begin();

            this.spriteBatch.Draw(texture, new Rectangle(0, 0, 200, 60), Color.White);
            int readableEnergy = (int)Math.Floor((energy.CurrentEnergy / Constants.MAX_ENERGY) * 100.0);
            this.spriteBatch.DrawString(font, readableEnergy.ToString(), new Vector2(100, 80), Color.Black);

            this.spriteBatch.End();
        }

        public void Dispose()
        {
        }
    }
}

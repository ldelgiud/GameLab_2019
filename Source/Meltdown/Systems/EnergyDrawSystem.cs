using Meltdown;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

class EnergyDrawSystem : DrawSystem {

    Energy energy;
    Texture2D texture;
    SpriteBatch spriteBatch;
    private SpriteFont font;

    public EnergyDrawSystem(Energy energy, Texture2D texture, SpriteBatch spriteBatch, SpriteFont font)
    {
        this.energy = energy;
        this.texture = texture;
        this.spriteBatch = spriteBatch;
        this.font = font;
    }

    public override void Draw(GameTime gameTime)
    {
        this.spriteBatch.Begin();

        this.spriteBatch.Draw(texture, new Rectangle(0,0,200,60), Color.White);
        int readableEnergy = (int) Math.Floor((energy.CurrentEnergy / Energy.maxEnergy)*100.0);
        this.spriteBatch.DrawString(font,readableEnergy.ToString(), new Vector2(100, 80), Color.Black);
        this.spriteBatch.End();
    }
}
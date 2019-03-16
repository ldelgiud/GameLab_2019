using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Components;
using Microsoft.Xna.Framework;
using Nez;

namespace Meltdown.Systems
{
    //TODO : Change to entitydrawsystem
    class TextureSystem : EntityDrawSystem
    {
        SpriteBatch spriteBatch;



        public TextureSystem(Matcher matcher) : base ( matcher )
        {
            
        }
        

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            foreach (int id in this.ActiveEntities)
            {
                PositionComponent position = this.positionMapper.Get(id);
                TextureComponent texture = this.textureMapper.Get(id);

                this.spriteBatch.Draw(texture.Texture, position.position, Color.White);
            }
            this.spriteBatch.End();
        }

    }
}

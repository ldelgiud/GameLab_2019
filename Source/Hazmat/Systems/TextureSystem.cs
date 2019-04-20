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

namespace Meltdown.Systems
{
    class TextureSystem : EntityDrawSystem
    {
        SpriteBatch spriteBatch;

        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<TextureComponent> textureMapper;

        public TextureSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(PositionComponent), typeof(TextureComponent)))
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
            this.textureMapper = mapperService.GetMapper<TextureComponent>();
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

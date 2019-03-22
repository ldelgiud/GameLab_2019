using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;

namespace Meltdown.Systems
{
    sealed class TextureDrawSystem : AEntitySystem<GameTime>
    {
        SpriteBatch spriteBatch;

        public TextureDrawSystem(World world, SpriteBatch spriteBatch) : base(
            world.GetEntities()
            .With<TransformComponent>()
            .With<TextureComponent>()
            .Build())
        {
            this.spriteBatch = spriteBatch;
        }

        protected override void PreUpdate(GameTime state)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(GameTime state, in Entity entity)
        {
            ref TransformComponent position = ref entity.Get<TransformComponent>();
            ref TextureComponent texture = ref entity.Get<TextureComponent>();

            //this.spriteBatch.Draw(texture.texture, position.TransformPoint(Vector2.Zero), Color.White);
            this.spriteBatch.Draw(texture: texture.texture, position: position.Position, rotation: position.Rotation, scale: position.Scale);
        }

        protected override void PostUpdate(GameTime state)
        {
            this.spriteBatch.End();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    sealed class TextureDrawSystem : AEntitySystem<Time>
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

        protected override void PreUpdate(Time time)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref TransformComponent transform = ref entity.Get<TransformComponent>();
            ref TextureComponent texture = ref entity.Get<TextureComponent>();

            //this.spriteBatch.Draw(texture.texture, position.TransformPoint(Vector2.Zero), Color.White);
            this.spriteBatch.Draw(texture: texture.texture, position: transform.Position, rotation: transform.Rotation, scale: transform.Scale);
        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

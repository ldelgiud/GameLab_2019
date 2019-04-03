using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;


namespace Meltdown.Systems
{
    sealed class TextureDrawSystem : AEntitySystem<Time>
    {
        Camera2D camera;
        SpriteBatch spriteBatch;

        public TextureDrawSystem(GraphicsDevice graphicsDevice, Camera2D camera, World world) : base(
            world.GetEntities()
            .With<WorldSpaceComponent>()
            .With<Transform2DComponent>()
            .With<Texture2DComponent>()
            .Build())
        {
            this.camera = camera;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        protected override void PreUpdate(Time time)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref Transform2DComponent transform = ref entity.Get<Transform2DComponent>();
            ref Texture2DComponent texture = ref entity.Get<Texture2DComponent>();

            var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform.value, texture.info);

            this.spriteBatch.Draw(
                texture: texture.value,
                position: position,
                rotation: rotation,
                scale: scale,
                origin: texture.value.Bounds.Size.ToVector2() / 2
                );

        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Meltdown.Graphics;

namespace Meltdown.Systems
{
    class ScreenTextureSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera camera;
        SpriteBatch spriteBatch;

        public ScreenTextureSystem(GraphicsDevice graphicsDevice, Camera camera, World world) : base(
            world.GetEntities()
            .With<ScreenTransformComponent>()
            .With<TextureComponent>()
            .With<BoundingRectangleComponent>()
            .Build()
            )
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        protected override void PreUpdate(Time state)
        {
            this.spriteBatch.Begin();
        }
        
        protected override void Update(Time time, in Entity entity)
        {
            ref TextureComponent texture = ref entity.Get<TextureComponent>();
            ref ScreenTransformComponent transform = ref entity.Get<ScreenTransformComponent>();
            ref BoundingRectangleComponent bounds = ref entity.Get<BoundingRectangleComponent>();

            var (position, rotation, scale, origin) = this.camera.ToScreenCoordinates(transform.value.GlobalTransform, texture.value.Bounds);

            // Override scaling to ignore shear from projection
            scale = transform.value.scale.ToVector2();

            this.spriteBatch.Draw(
                texture: texture.value,
                position: position,
                color: Color.White,
                rotation: rotation,
                scale: scale,
                origin: origin
                );
        }

        protected override void PostUpdate(Time state)
        {
            this.spriteBatch.End();
        }
    }
}

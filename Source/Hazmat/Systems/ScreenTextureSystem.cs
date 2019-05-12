using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.Graphics;

namespace Hazmat.Systems
{
    class ScreenTextureSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera2D camera;
        SpriteBatch spriteBatch;

        public ScreenTextureSystem(GraphicsDevice graphicsDevice, Camera2D camera, World world) : base(
            world.GetEntities()
            .With<ScreenSpaceComponent>()
            .With<Transform2DComponent>()
            .With<Texture2DComponent>()
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
            ref Texture2DComponent texture = ref entity.Get<Texture2DComponent>();
            ref Transform2DComponent transform = ref entity.Get<Transform2DComponent>();

            var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform.value, texture.info);

            if (texture.rotated)
            {
                var temp = scale;
                scale.X = temp.Y;
                scale.Y = temp.X;
            }

            var bounds = texture.info.bounds ?? texture.value.Bounds;
            var origin = bounds.Size.ToVector2() / 2;

            this.spriteBatch.Draw(
                sourceRectangle: bounds,
                texture: texture.value,
                position: position,
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

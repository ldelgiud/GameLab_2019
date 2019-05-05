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
    class ScreenTextSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera2D camera;
        SpriteBatch spriteBatch;

        public ScreenTextSystem(GraphicsDevice graphicsDevice, Camera2D camera, World world) : base(
            world.GetEntities()
            .With<ScreenSpaceComponent>()
            .With<Transform2DComponent>()
            .With<TextComponent>()
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
            ref TextComponent text = ref entity.Get<TextComponent>();
            ref Transform2DComponent transform = ref entity.Get<Transform2DComponent>();

            var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform.value, text.info);

            var origin = text.font.MeasureString(text.text) / 2;

            this.spriteBatch.DrawString(
                spriteFont: text.font,
                text: text.text,
                position: position,
                rotation: rotation,
                scale: scale,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: 0,
                color: text.info.color
                );
        }

        protected override void PostUpdate(Time state)
        {
            this.spriteBatch.End();
        }
    }
}

using System.Diagnostics;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class ScreenTextureSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Screen screen;
        SpriteBatch spriteBatch;

        public ScreenTextureSystem(GraphicsDevice graphicsDevice, World world, Screen screen) : base(
            world.GetEntities()
            .With<ScreenTransformComponent>()
            .With<TextureComponent>()
            .With<BoundingRectangleComponent>()
            .Build()
            )
        {
            this.graphicsDevice = graphicsDevice;
            this.screen = screen;
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

            this.spriteBatch.Draw(texture.value,
                destinationRectangle: this.screen.Project(transform.Translation, transform.Scaling, bounds.value),
                color: Color.White
                //rotation: rotation
                );
        }

        protected override void PostUpdate(Time state)
        {
            this.spriteBatch.End();
        }
    }
}

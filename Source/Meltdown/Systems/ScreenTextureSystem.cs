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
        GameWindow window;   
        GraphicsDevice graphicsDevice;
        Camera camera;
        SpriteBatch spriteBatch;

        public ScreenTextureSystem(GameWindow window, GraphicsDevice graphicsDevice, Camera camera, World world) : base(
            world.GetEntities()
            .With<ScreenTransformComponent>()
            .With<TextureComponent>()
            .With<BoundingRectangleComponent>()
            .Build()
            )
        {
            this.window = window;
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

            var rotation = transform.value.rotation.Z;
            var scale = new Vector2(transform.value.scale.X, transform.value.scale.Y);

            var mvp = camera.Projection * camera.View * transform.value.GlobalTransform;

            this.spriteBatch.Draw(
                texture: texture.value,
                position: this.camera.ToScreenCoordinates(this.window.ClientBounds, mvp, texture.value.Bounds),
                color: Color.White,
                rotation: rotation,
                scale: scale
                );
        }

        protected override void PostUpdate(Time state)
        {
            this.spriteBatch.End();
        }
    }
}

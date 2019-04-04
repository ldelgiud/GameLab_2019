using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Graphics;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class AABBDebugDrawSystem : AEntitySystem<Time>
    {
        SpriteBatch spriteBatch;
        Camera2D camera;

        Texture2D debugBoxTex;

        public AABBDebugDrawSystem(World world, GraphicsDevice graphicsDevice, Camera2D camera, Texture2D debugBoxTex) : base(
            world.GetEntities()
            .With<AABBComponent>()
            .Build())
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.camera = camera;
            this.debugBoxTex = debugBoxTex;
        }


        protected override void PreUpdate(Time time)
        {
            this.spriteBatch.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref var aabbComponent = ref entity.Get<AABBComponent>();
            var size = aabbComponent.element.Span.UpperBound - aabbComponent.element.Span.LowerBound;
            var transform = new Transform2D(aabbComponent.element.Span.Center);

            var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform, new Texture2DInfo(null, scale: size / this.debugBoxTex.Bounds.Size.ToVector2()));

            spriteBatch.Draw(
                texture: this.debugBoxTex,
                position: position,
                rotation: rotation,
                scale: scale,
                origin: this.debugBoxTex.Bounds.Size.ToVector2() / 2
                );


        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

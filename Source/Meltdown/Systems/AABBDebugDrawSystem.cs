using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class AABBDebugDrawSystem : AEntitySystem<Time>
    {
        SpriteBatch spriteBatch;
        Camera camera;

        Texture2D debugBoxTex;

        public AABBDebugDrawSystem(World world, GraphicsDevice graphicsDevice, Camera camera, Texture2D debugBoxTex) : base(
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
            ref AABBComponent aabbComponent = ref entity.Get<AABBComponent>();
            
            Vector2 size = new Vector2(aabbComponent.element.Span.Width,
                                       aabbComponent.element.Span.Height);

            // Position
            Vector2 upperLeftPos = aabbComponent.element.Span.LowerBound + Vector2.UnitY * aabbComponent.element.Span.Height;

            spriteBatch.Draw(debugBoxTex,
                new Rectangle(camera.InverseProjectPoint(upperLeftPos), size.ToPoint()),
                Color.White
                );


        }

        protected override void PostUpdate(Time time)
        {
            this.spriteBatch.End();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Spine;

using Meltdown.Components;
using Meltdown.Graphics;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class SpineSkeletonDrawSystem<T> : AEntitySystem<Time>
    {
        Camera2D camera;
        SkeletonRenderer skeletonRenderer;
        SkeletonDebugRenderer skeletonDebugRenderer;


        public SpineSkeletonDrawSystem(GraphicsDevice graphicsDevice, Camera2D camera, World world) : base (
            world.GetEntities()
            .With<SkeletonComponent>()
            .With<Transform2DComponent>()
            .With<T>()
            .Build()
            )
        {
            this.camera = camera;
            this.skeletonRenderer = new SkeletonRenderer(graphicsDevice);
            this.skeletonDebugRenderer = new SkeletonDebugRenderer(graphicsDevice);
        }

        protected override void PreUpdate(Time state)
        {

            var cameraTranslation = Camera2D.WorldToPerspective(this.camera.Transform.Translation);
            var v = Matrix.CreateLookAt(new Vector3(cameraTranslation, 50), cameraTranslation.ToVector3(), Vector3.UnitY);
            var p = Matrix.CreateOrthographic(this.camera.ScreenWidth, this.camera.ScreenHeight, 0, 100);

            ((BasicEffect)this.skeletonRenderer.Effect).Projection = p;
            ((BasicEffect)this.skeletonRenderer.Effect).View = v;
            //this.skeletonDebugRenderer.Effect.Projection = p;
            //this.skeletonDebugRenderer.Effect.View = v;

            this.skeletonRenderer.Begin();
            //this.skeletonDebugRenderer.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref var skeleton = ref entity.Get<SkeletonComponent>();

            this.skeletonRenderer.Draw(skeleton.value);
            //this.skeletonDebugRenderer.Draw(skeleton.value);
        }

        protected override void PostUpdate(Time state)
        {
            this.skeletonRenderer.End();
            //this.skeletonDebugRenderer.End();
        }
    }
}

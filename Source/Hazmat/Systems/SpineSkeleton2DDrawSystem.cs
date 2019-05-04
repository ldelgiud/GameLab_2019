using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Spine;

using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class SpineSkeleton2DDrawSystem<T> : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera2D camera;
        SkeletonRenderer skeletonRenderer;
        SkeletonDebugRenderer skeletonDebugRenderer;


        public SpineSkeleton2DDrawSystem(GraphicsDevice graphicsDevice, Camera2D camera, World world) : base(
            world.GetEntities()
            .With<SpineSkeletonComponent>()
            .With<Transform2DComponent>()
            .With<T>()
            .Build()
            )
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            this.skeletonRenderer = new SkeletonRenderer(graphicsDevice);
            this.skeletonDebugRenderer = new SkeletonDebugRenderer(graphicsDevice);
        }

        protected override void PreUpdate(Time state)
        {
            var v = Matrix.CreateLookAt(new Vector3(this.camera.Transform.Translation, 50), this.camera.Transform.Translation.ToVector3(), Vector3.UnitY);
            var p = Matrix.CreateOrthographic(this.camera.ScreenWidth, this.camera.ScreenHeight, 0, 100);

            ((BasicEffect)this.skeletonRenderer.Effect).Projection = p;
            ((BasicEffect)this.skeletonRenderer.Effect).View = v;
            //this.skeletonDebugRenderer.Effect.Projection = p;
            //this.skeletonDebugRenderer.Effect.View = v;

            ((BasicEffect)this.skeletonRenderer.Effect).Projection = p;
            ((BasicEffect)this.skeletonRenderer.Effect).View = v;
            //this.skeletonDebugRenderer.Effect.Projection = p;
            //this.skeletonDebugRenderer.Effect.View = v;

            this.skeletonRenderer.Begin();
            //this.skeletonDebugRenderer.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref var skeleton = ref entity.Get<SpineSkeletonComponent>();
            ref var transform = ref entity.Get<Transform2DComponent>();

            var m = transform.value.TransformMatrix.ToMatrix();
            ((BasicEffect)this.skeletonRenderer.Effect).World = m;

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

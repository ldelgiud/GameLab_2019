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
    class SpineSkeleton3DDrawSystem<T> : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera3D camera;
        SkeletonRenderer skeletonRenderer;
        SkeletonDebugRenderer skeletonDebugRenderer;


        public SpineSkeleton3DDrawSystem(GraphicsDevice graphicsDevice, Camera3D camera, World world) : base (
            world.GetEntities()
            .With<SpineSkeletonComponent>()
            .With<Transform3DComponent>()
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
            var v = this.camera.View;
            var p = this.camera.Projection;

            ((BasicEffect)this.skeletonRenderer.Effect).Projection = p;
            ((BasicEffect)this.skeletonRenderer.Effect).View = v;
            //this.skeletonDebugRenderer.Effect.Projection = p;
            //this.skeletonDebugRenderer.Effect.View = v;

            
            //this.skeletonRenderer.Begin();
            //this.skeletonDebugRenderer.Begin();
        }

        protected override void Update(Time time, in Entity entity)
        {
            this.skeletonRenderer.Begin();
            this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            ref var skeleton = ref entity.Get<SpineSkeletonComponent>();
            ref var transform = ref entity.Get<Transform3DComponent>();

            // Custom model matrix to Billboard the skeleton to the screen so that its not flat
            var m =
                Matrix.CreateScale(skeleton.info.scale * transform.value.Scale * new Vector3(-1, 1, 1)) *
                Matrix.CreateRotationX(transform.value.Rotation.X) *
                Matrix.CreateRotationY(transform.value.Rotation.Y) *
                Matrix.CreateRotationZ(transform.value.Rotation.Z) *
                Matrix.CreateBillboard(Vector3.Zero, this.camera.distance * Camera3D.ISOMETRIC_OFFSET, Camera3D.ISOMETRIC_UP, Camera3D.ISOMETRIC_OFFSET) *
                Matrix.CreateTranslation(skeleton.info.translation + transform.value.Translation);
            ((BasicEffect)this.skeletonRenderer.Effect).World = m;

            this.skeletonRenderer.Draw(skeleton.value);
            this.skeletonRenderer.End();
            //this.skeletonDebugRenderer.Draw(skeleton.value);
        }

        protected override void PostUpdate(Time state)
        {
            //this.skeletonRenderer.End();
            //this.skeletonDebugRenderer.End();
        }
    }
}

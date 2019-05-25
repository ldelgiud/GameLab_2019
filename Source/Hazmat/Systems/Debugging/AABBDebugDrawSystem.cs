using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.PostProcessor;

namespace Hazmat.Systems
{
    class AABBDebugDrawSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera3D camera;

        Texture2D debugBoxTex;

        BasicEffect effect;
        RasterizerState rasterizerState;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public AABBDebugDrawSystem(World world, GraphicsDevice graphicsDevice, Camera3D camera, Texture2D debugBoxTex) : base(
            world.GetEntities()
            .With<AABBComponent>()
            .Build())
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            this.debugBoxTex = debugBoxTex;

            this.effect = new BasicEffect(graphicsDevice);

            this.rasterizerState = new RasterizerState();
            this.rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;

            this.vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 0)),
            });

            this.indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(new short[] {
                0, 1, 2,
                0, 2, 3,
            });
        }

        protected override void PreUpdate(Time time)
        {
            var v = this.camera.View;
            var p = this.camera.Projection;

            this.effect.View = v;
            this.effect.Projection = p;
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.debugBoxTex;
        }

        protected override void Update(Time time, in Entity entity)
        {
            /*   
            ref var aabbComponent = ref entity.Get<AABBComponent>();
            var size = aabbComponent.element.Span.UpperBound - aabbComponent.element.Span.LowerBound;

            var m = Matrix.CreateScale(aabbComponent.element.Span.Width / 2, aabbComponent.element.Span.Height / 2, 1) *
                Matrix.CreateTranslation(new Vector3(aabbComponent.element.Span.Center, Constants.LAYER_BACKGROUND_DEBUG));
            this.effect.World = m;

            this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
            this.graphicsDevice.Indices = this.indexBuffer;

            this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.graphicsDevice.RasterizerState = this.rasterizerState;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
            */
        }

        protected override void PostUpdate(Time state)
        {
            RenderCapture renderCapture = Hazmat.Instance.ActiveState.GetInstance<RenderCapture>();
            PostProcessing postProcessor = Hazmat.Instance.ActiveState.GetInstance<PostProcessing>();
            renderCapture.End();
            postProcessor.Input = renderCapture.GetTexture();
            postProcessor.Draw();
        }
    }
}

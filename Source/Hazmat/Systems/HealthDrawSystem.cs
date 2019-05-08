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
    class HealthDrawSystem : AEntitySystem<Time>
    {

        GraphicsDevice graphicsDevice;
        Camera3D camera;

        Texture2D healthBar_W;
        Texture2D healthBar_G;

        BasicEffect effect;
        RasterizerState rasterizerState;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        

        public HealthDrawSystem(World world, GraphicsDevice graphicsDevice, Camera3D camera, Texture2D healthBar_W, Texture2D healthBar_G) : base(
          world.GetEntities()
          .With<HealthComponent>()
          .With<Transform3DComponent>()
          .Build())
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            this.healthBar_W = healthBar_W;
            this.healthBar_G = healthBar_G;

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
            this.effect.Texture = this.healthBar_G;
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref HealthComponent healtComponent = ref entity.Get<HealthComponent>();
            ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();
            
            // Custom model matrix to Billboard the skeleton to the screen so that its not flat
            var m =
                Matrix.CreateScale(transform.value.Scale * new Vector3(-1, 1, 1)) * Matrix.CreateScale(1.5f, 0.3f, 1f) *
                Matrix.CreateRotationX(transform.value.LocalRotation.X) *
                Matrix.CreateRotationY(transform.value.LocalRotation.Y) *
                //Matrix.CreateRotationZ(transform.value.LocalRotation.Z) * // Removed to clamp health bars to fixed rotation
                Matrix.CreateBillboard(Vector3.Zero, this.camera.distance * Camera3D.ISOMETRIC_OFFSET, Camera3D.ISOMETRIC_UP, Camera3D.ISOMETRIC_OFFSET) *
                Matrix.CreateTranslation(transform.value.Translation) * Matrix.CreateTranslation(0, 0, 6f);

            this.effect.World = Matrix.CreateScale(healtComponent.Health / healtComponent.TotalHealth, 1, 1) * m;

            this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
            this.graphicsDevice.Indices = this.indexBuffer;

            this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.graphicsDevice.RasterizerState = this.rasterizerState;

            foreach (var pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }

            // Second sprite
            //this.effect.Texture = healthBar_G;


            //this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
            //this.graphicsDevice.Indices = this.indexBuffer;

            //this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            //this.graphicsDevice.RasterizerState = this.rasterizerState;


            //this.effect.World = Matrix.CreateScale(healtComponent.Health / healtComponent.TotalHealth, 1, 1) * m;

            //foreach (var pass in this.effect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            //}

        }
        
    }
}

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class TileMapDrawSystem : ISystem<Time>
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        BasicEffect effect;
        RasterizerState rasterizerState;

        Camera3D camera;
        TileMap tileMap;

        public bool IsEnabled { get; set; } = true;

        public TileMapDrawSystem(GraphicsDevice graphicsDevice, Camera3D camera, TileMap tileMap)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.camera = camera;
            this.tileMap = tileMap;
            this.effect = new BasicEffect(graphicsDevice);
            this.rasterizerState = new RasterizerState();
            this.rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
        }

        public void Update(Time state)
        {
            var cameraPosition = this.camera.Transform.Translation;
            var aabb = new AABB(
                new Vector2(cameraPosition.X - this.camera.width * 2, cameraPosition.Y - this.camera.height * 2),
                new Vector2(cameraPosition.X + this.camera.width * 2, cameraPosition.Y + this.camera.height * 2)
                );

            this.effect.View = this.camera.View;
            this.effect.Projection = this.camera.Projection;
            this.effect.TextureEnabled = true;

            this.tileMap.quadtree.QueryAABB((element) =>
            {
                var entity = element.Value;
                ref var transform = ref entity.Get<Transform3DComponent>();
                ref var tile = ref entity.Get<TileComponent>();

                this.graphicsDevice.SetVertexBuffer(tile.vertexBuffer);
                this.graphicsDevice.Indices = tile.indexBuffer;

                this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
                this.graphicsDevice.RasterizerState = this.rasterizerState;

                // Prevent white outline for background details
                this.graphicsDevice.BlendState = BlendState.NonPremultiplied;

                this.effect.World = transform.value.TransformMatrix;
                this.effect.Texture = tile.texture;                

                foreach (var pass in this.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
                }

                return true;
            }, ref aabb);
        }

        public void Dispose()
        {

        }
    }
}

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Spine;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
{
    class TileModelResourceManager : AResourceManager<TileModelInfo, TileModelAlias>
    {
        GraphicsDevice graphicsDevice;
        Atlas atlas;

        public TileModelResourceManager(GraphicsDevice graphicsDevice, String path)
        {
            this.graphicsDevice = graphicsDevice;
            this.atlas = new Atlas(@"Content\" + path + ".atlas", new XnaTextureLoader(graphicsDevice));
        }

        protected override TileModelAlias Load(TileModelInfo info)
        {
            var region = this.atlas.FindRegion(info.name);
            var texture = (Texture2D)this.atlas.pages[0].rendererObject;
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            var indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);

            var size = texture.Bounds;

            // Get x and y based on rotation and normalized to unit square
            //var top_right = new Vector3(MathF.Cos(info.rotation + MathF.PI / 4), MathF.Sin(info.rotation + MathF.PI / 4), 0) * MathF.Sqrt(2);
            //var top_left = new Vector3(MathF.Cos(info.rotation + MathF.PI / 4 + MathF.PI / 2), MathF.Sin(info.rotation + MathF.PI / 4 + MathF.PI / 2), 0) * MathF.Sqrt(2);
            //var bottom_left = new Vector3(MathF.Cos(info.rotation + MathF.PI / 4 + 2 * MathF.PI / 2), MathF.Sin(info.rotation + MathF.PI / 4 + 2 * MathF.PI / 2), 0) * MathF.Sqrt(2);
            //var bottom_right = new Vector3(MathF.Cos(info.rotation + MathF.PI / 4 + 3 * MathF.PI / 2), MathF.Sin(info.rotation + MathF.PI / 4 + 3 * MathF.PI / 2), 0) * MathF.Sqrt(2);


            var top = (float)region.y / (float)size.Height;
            var right = (float)(region.x + (region.rotate ? region.height : region.width)) / (float)size.Width;
            var bottom = (float)(region.y + (region.rotate ? region.width : region.height)) / (float)size.Height;
            var left = (float)region.x / (float)size.Width;

            // Invert top/bottom to correct for uv coordinates
            vertexBuffer.SetData(new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(left, bottom)),
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(left, top)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(right, top)),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(right, bottom)),
            });
            //vertexBuffer.SetData(new VertexPositionTexture[] {
            //    new VertexPositionTexture(bottom_left, new Vector2(right, bottom)),
            //    new VertexPositionTexture(top_left, new Vector2(left, bottom)),
            //    new VertexPositionTexture(top_right, new Vector2(left, top)),
            //    new VertexPositionTexture(bottom_right, new Vector2(right, top)),
            //});

            indexBuffer.SetData(new short[] {
                0, 1, 2,
                0, 2, 3,
            });

            return new TileModelAlias(texture, vertexBuffer, indexBuffer);
        }

        protected override void OnResourceLoaded(in Entity entity, TileModelInfo info, TileModelAlias resource)
        {
            entity.Set(new TileComponent(resource.texture, resource.vertexBuffer, resource.indexBuffer));
        }
    }
}

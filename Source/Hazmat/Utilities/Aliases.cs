using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Spine;

namespace Hazmat.Utilities
{
    struct ModelAlias : IDisposable
    {
        public Model value;

        public void Dispose()
        {

        }
    }

    struct Texture2DAlias : IDisposable
    {
        public Texture2D value;
        public Rectangle? bounds;

        public Texture2DAlias(Texture2D texture, Rectangle? bounds = null)
        {
            this.value = texture;
            this.bounds = bounds;
        }

        public void Dispose()
        {

        }
    }

    struct TextAlias : IDisposable
    {
        public SpriteFont font;

        public TextAlias(SpriteFont font)
        {
            this.font = font;
        }

        public void Dispose()
        {

        }
    }

    struct SkeletonDataAlias : IDisposable
    {
        public SkeletonData skeletonData;

        public SkeletonDataAlias(SkeletonData skeletonData)
        {
            this.skeletonData = skeletonData;
        }


        public void Dispose()
        {

        }

    }

    struct AtlasTextureAlias : IDisposable
    {
        public Texture2D value;
        public Rectangle bounds;
        public bool rotate;

        public AtlasTextureAlias(Texture2D texture, Rectangle bounds, bool rotate = false)
        {
            this.value = texture;
            this.bounds = bounds;
            this.rotate = rotate;
        }

        public void Dispose()
        {

        }
    }

    struct TileModelAlias : IDisposable
    {
        public Texture2D texture;
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public TileModelAlias(Texture2D texture, VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            this.texture = texture;
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;
        }

        public void Dispose() { }
    }
}

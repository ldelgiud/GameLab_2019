using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Spine;

namespace Meltdown.Utilities
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
}

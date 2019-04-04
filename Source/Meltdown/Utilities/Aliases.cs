using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public void Dispose()
        {

        }
    }
}

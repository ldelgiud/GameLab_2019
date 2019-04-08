using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Utilities;

namespace Meltdown.Components
{
    struct Texture2DComponent
    {
        public Texture2D value;
        public Texture2DInfo info;

        public Texture2DComponent(Texture2D value, Texture2DInfo info)
        {
            this.value = value;
            this.info = info;
        }
    }
}

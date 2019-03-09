using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Components
{
    class TextureComponent
    {
        public Texture2D Texture { get; }

        public TextureComponent(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}

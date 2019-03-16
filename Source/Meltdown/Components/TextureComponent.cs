using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Meltdown.Components
{
    class TextureComponent : RenderableComponent
    {
        public Texture2D Texture { get; }

        public TextureComponent(Texture2D texture)
        {
            this.Texture = texture;
        }

        public override void render(Graphics graphics, Camera camera)
        {
            throw new NotImplementedException();
        }
    }
}

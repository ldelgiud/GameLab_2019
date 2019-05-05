using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hazmat.Utilities;

namespace Hazmat.Components
{
    struct TextComponent
    {
        public TextInfo info;
        public String text;
        public SpriteFont font;

        public TextComponent(TextInfo info, String text, SpriteFont font)
        {
            this.info = info;
            this.text = text;
            this.font = font;
        }
    }
}

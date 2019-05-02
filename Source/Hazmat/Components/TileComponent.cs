using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hazmat.Components
{
    struct TileComponent
    {
        public Texture2D texture;
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public TileComponent(Texture2D texture, VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            this.texture = texture;
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;
        }
    }
}

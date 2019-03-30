using System;

using Microsoft.Xna.Framework;

namespace Meltdown.Components
{
    public struct BoundingRectangleComponent
    {
        public Rectangle value;

        public BoundingRectangleComponent(int width, int height)
        {
            this.value = new Rectangle(-width / 2, -height / 2, width, height);
        }

        public BoundingRectangleComponent(Rectangle rectangle)
        {
            this.value = rectangle;
        }
    }
}

using System;

using Microsoft.Xna.Framework;

namespace Meltdown.Components
{
    struct BoundingBoxComponent
    {
        public BoundingBox value;

        public BoundingBoxComponent(float width, float height, float depth)
        {
            this.value = new BoundingBox(
                new Vector3(-width / 2, -height / 2, -depth / 2),
                new Vector3(width / 2, height / 2, depth / 2)
                );
        }

        public BoundingBoxComponent(BoundingBox value)
        {
            this.value = value;
        }
    }
}

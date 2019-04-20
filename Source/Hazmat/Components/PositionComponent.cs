using Microsoft.Xna.Framework;

namespace Hazmat.Components
{
    struct PositionComponent
    {
        public Vector2 position;

        public PositionComponent(Vector2 position)
        {
            this.position = position;
        }
    }
}

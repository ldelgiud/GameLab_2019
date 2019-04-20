using Microsoft.Xna.Framework;

namespace Hazmat.Components
{
    struct VelocityComponent
    {
        public Vector2 velocity;

        public VelocityComponent(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }
}

using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Meltdown.Collision
{
    /// <summary>
    /// Data for a collision event
    /// </summary>
    struct CollisionEvent
    {
        /// <summary>
        /// Entity that moved
        /// </summary>
        public Entity collider;

        /// <summary>
        /// Entity that was collided
        /// </summary>
        public Entity collidee;

        public CollisionEvent(Entity collider, Entity collidee, Vector2 penetrationVector)
        {
            this.collider = collider;
            this.collidee = collidee;
        }
    }
}

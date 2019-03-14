using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;

namespace Meltdown.Collision
{
    /// <summary>
    /// Static class that holds all collision handlers
    /// </summary>
    static class CollisionHandlers
    {
        /// <summary>
        /// Collision handler type
        /// </summary>
        /// <param name="world"></param>
        /// <param name="collider"></param>
        /// <param name="collidee"></param>
        /// <param name="penetrationVector"></param>
        public delegate void CollisionHandler(World world, Entity collider, Entity collidee, Vector2 penetrationVector);

        /// <summary>
        /// Const list of collision handlers
        /// </summary>
        public static readonly CollisionHandler[] COLLISION_HANDLERS = {
        };

    }
}

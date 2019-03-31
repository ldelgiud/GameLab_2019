using System;
using System.Collections.Generic;


using DefaultEcs;

namespace Meltdown.Collision
{
    /// <summary>
    /// Base class for collision handling.
    /// </summary>
    abstract class CollisionHandler
    {
        bool commutative;
        EntitySet colliderSet;
        EntitySet collideeSet;

        public CollisionHandler(EntitySet colliders, EntitySet collidees, bool commutative = false)
        {
            this.colliderSet = colliders;
            this.collideeSet = collidees;
            this.commutative = commutative;
        }

        public void HandleCollisions(CollisionType type, IEnumerable<(Entity, Entity)> entities)
        {
            var colliders = this.colliderSet.GetEntities();
            var collidees = this.collideeSet.GetEntities();
            foreach (var tuple in entities)
            {
                var (collider, collidee) = tuple;

                if (colliders.IndexOf(collider) != -1 && collidees.IndexOf(collidee) != -1)
                {
                    this.HandleCollision(type, collider, collidee);
                }

                if (this.commutative && colliders.IndexOf(collidee) != -1 && collidees.IndexOf(collider) != -1)
                {
                    this.HandleCollision(type, collidee, collider);
                }
            }
            
        }

        public abstract void HandleCollision(CollisionType type, Entity collider, Entity collidee);
    }
}

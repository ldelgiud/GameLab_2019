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
        EntitySet colliderSet;
        EntitySet collideeSet;

        public CollisionHandler(EntitySet colliders, EntitySet collidees)
        {
            this.colliderSet = colliders;
            this.collideeSet = collidees;
        }

        public void HandleCollisions(CollisionType type, IEnumerable<(Entity, Entity)> entities)
        {
            var colliders = this.colliderSet.GetEntities();
            var collidees = this.collideeSet.GetEntities();
            foreach (var tuple in entities)
            {
                var collider = tuple.Item1;
                var collidee = tuple.Item2;
                var colliderIndex = colliders.IndexOf(collider);
                var collideeIndex = collidees.IndexOf(collidee);

                if (colliderIndex != -1 && collideeIndex != -1)
                {
                    this.HandleCollision(type, collider, collidee);
                }
            }
            
        }

        public abstract void HandleCollision(CollisionType type, Entity collider, Entity collidee);
    }
}

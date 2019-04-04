using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Collision;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    /// <summary>
    /// System to run a set of collision handlers
    /// </summary>
    class CollisionSystem : ISystem<Time>, IDisposable, ICollisionSet
    {
        HashSet<(Entity, Entity)> start = new HashSet<(Entity, Entity)>();
        HashSet<(Entity, Entity)> ongoing = new HashSet<(Entity, Entity)>();
        HashSet<(Entity, Entity)> stop = new HashSet<(Entity, Entity)>();

        IEnumerable<CollisionHandler> collisionHandlers;

        public bool IsEnabled { get; set; } = true;

        public CollisionSystem(IEnumerable<CollisionHandler> collisionHandlers)
        {
            this.collisionHandlers = collisionHandlers;
        }

        public void AddCollision(Entity collider, Entity collidee)
        {
            this.start.Add((collider, collidee));
        }

        void HandleCollisions(CollisionType type, IEnumerable<(Entity, Entity)> entities)
        {
            foreach (var tuple in entities)
            {
                var (collider, collidee) = tuple;

                // Guard entities having died
                if (!collider.IsAlive || !collidee.IsAlive) continue;

                foreach (var collisionHandler in this.collisionHandlers)
                {
                    if (
                        collisionHandler.colliderTypes.All(component => collider.Has(component)) &&
                        collisionHandler.collideeTypes.All(component => collidee.Has(component))
                        )
                    {
                        collisionHandler.HandleCollision(type, collider, collidee);
                    }

                    if (
                        collisionHandler.commutative &&
                        collisionHandler.collideeTypes.All(component => collider.Has(component)) &&
                        collisionHandler.colliderTypes.All(component => collidee.Has(component))
                        )
                    {
                        collisionHandler.HandleCollision(type, collidee, collider);
                    }

                    
                }

            }
        }

        public void Update(Time gameTime)
        {
            // Stop all ongoing collisions
            this.stop.UnionWith(this.ongoing);

            // Unless they are renewed this tick
            this.stop.ExceptWith(this.start);

            // Remove ongoing collisions from start
            this.start.ExceptWith(this.ongoing);

            // Remove stopping collisions from ongoing
            this.ongoing.ExceptWith(this.stop);

            this.HandleCollisions(CollisionType.Start, this.start);
            this.HandleCollisions(CollisionType.Ongoing, this.ongoing);
            this.HandleCollisions(CollisionType.Stop, this.stop);

            // Started collisions become ongoing
            this.ongoing.UnionWith(this.start);

            // Clear start/stop collisions
            this.start.Clear();
            this.stop.Clear();

        }

        public void Dispose()
        {

        }
    }
}

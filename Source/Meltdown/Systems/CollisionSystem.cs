using System;
using System.Collections.Concurrent;

using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Collision;
using CollisionHandler = Meltdown.Collision.CollisionHandlers.CollisionHandler;

namespace Meltdown.Systems
{
    /// <summary>
    /// System for handling collisions
    /// </summary>
    class CollisionSystem : UpdateSystem
    {
        private World world;

        public ConcurrentQueue<CollisionEvent> EventQueue { get; } = new ConcurrentQueue<CollisionEvent>();

        public override void Initialize(World world)
        {
            base.Initialize(world);
            this.world = world;
        }

        public override void Update(GameTime gameTime)
        {
            // Iterate all collision events
            while (this.EventQueue.TryDequeue(out CollisionEvent collisionEvent))
            {
                // Iterate all collision handlers
                foreach (var collisionHandlerTuple in CollisionHandlers.COLLISION_HANDLERS)
                {
                    var colliderAspect = collisionHandlerTuple.Item1;
                    var collideeAspect = collisionHandlerTuple.Item2;
                    var collisionHandler = collisionHandlerTuple.Item3;

                    // Check if handler is interested
                    if (colliderAspect.IsInterested(collisionEvent.collider.ComponentBits) && collideeAspect.IsInterested(collisionEvent.collidee.ComponentBits)) {
                        // Handle collision
                        collisionHandler(this.world, collisionEvent.collider, collisionEvent.collidee);
                    }
                }
            }
        }

    }
}

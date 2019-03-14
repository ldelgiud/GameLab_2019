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
                foreach (CollisionHandler collisionHandler in CollisionHandlers.COLLISION_HANDLERS)
                {
                    // Handle collision
                    collisionHandler(this.world, collisionEvent.collider, collisionEvent.collidee, collisionEvent.penetrationVector);
                }
            }
        }

    }
}

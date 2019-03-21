using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Collision;

namespace Meltdown.Systems
{
    /// <summary>
    /// System to run a set of collision handlers
    /// </summary>
    class CollisionSystem : ISystem<GameTime>, IDisposable, ICollisionSet
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

        public void Update(GameTime gameTime)
        {
            // Stop all ongoing collisions
            this.stop.UnionWith(this.ongoing);

            // Unless they are renewed this tick
            this.stop.ExceptWith(this.start);

            // Remove ongoing collisions from start
            this.start.ExceptWith(this.ongoing);

            // Remove stopping collisions from ongoing
            this.ongoing.ExceptWith(this.stop);

            foreach (var collisionHandler in this.collisionHandlers)
            {
                collisionHandler.HandleCollisions(CollisionType.Start, this.start);
                collisionHandler.HandleCollisions(CollisionType.Ongoing, this.ongoing);
                collisionHandler.HandleCollisions(CollisionType.Stop, this.stop);
            }

            
            // Started collisions become ongoing
            this.ongoing.UnionWith(this.start);

            // Clear start/stop collisions
            start.Clear();
            stop.Clear();
        }

        public void Dispose()
        {

        }
    }
}

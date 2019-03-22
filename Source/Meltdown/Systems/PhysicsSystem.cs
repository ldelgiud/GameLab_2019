using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Meltdown.Collision;
using Meltdown.Components;

namespace Meltdown.Systems
{
    sealed class PhysicsSystem : AEntitySystem<GameTime>
    {
        public QuadTree<Entity> quadtree;
        ICollisionSet collisionSet;

        public PhysicsSystem(World world, ICollisionSet collisionSet) : base(
            world.GetEntities()
            .With<TransformComponent>()
            .With<VelocityComponent>()
            .Build()) {
            this.collisionSet = collisionSet;
            this.quadtree = new QuadTree<Entity>(
                new AABB() {
                    LowerBound = new Vector2(-10000, -10000),
                    UpperBound = new Vector2(10000, 10000) },
                10, 7);
        }

        protected override void Update(GameTime state, in Entity entity)
        {
            ref TransformComponent position = ref entity.Get<TransformComponent>();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();

            bool collision = false;
            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                Element<Entity> element = aabb.element;

                AABB target = new AABB {
                    LowerBound = element.Span.LowerBound + velocity.velocity * (state.ElapsedGameTime.Milliseconds / 1000f),
                    UpperBound = element.Span.UpperBound + velocity.velocity * (state.ElapsedGameTime.Milliseconds / 1000f)
                };

                List<Entity> collisions = new List<Entity>();
                this.quadtree.QueryAABB((Element<Entity> collidee) =>
                {
                    if (collidee == element)
                    {
                        return true;
                    }
                    collisions.Add(collidee.Value);
                    collision = true;
                    return false;
                }, ref target);

                foreach(var collidee in collisions)
                {
                    this.collisionSet.AddCollision(entity, collidee);
                }

                if (!collision)
                {
                    quadtree.RemoveNode(element);
                    element.Span = target;
                    quadtree.AddNode(element);
                }
            }

            if (!collision)
            {
                position.LocalPosition += velocity.velocity * (state.ElapsedGameTime.Milliseconds / 1000f);
            }
        }
    }
}

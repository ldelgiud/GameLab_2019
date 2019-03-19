using System.Diagnostics;
using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Meltdown.Components;

namespace Meltdown.Systems
{
    sealed class PhysicsSystem : AEntitySystem<GameTime>
    {
        public QuadTree<Entity> quadtree;

        public PhysicsSystem(World world) : base(
            world.GetEntities()
            .With<PositionComponent>()
            .With<VelocityComponent>()
            .Build()) {
            this.quadtree = new QuadTree<Entity>(
                new AABB() {
                    LowerBound = new Vector2(-10000, -10000),
                    UpperBound = new Vector2(10000, 10000) },
                10, 7);
        }

        protected override void Update(GameTime state, in Entity entity)
        {
            ref PositionComponent position = ref entity.Get<PositionComponent>();
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

                this.quadtree.QueryAABB((Element<Entity> collidee) =>
                {
                    if (collidee == element)
                    {
                        Debug.WriteLine("SELF COLLISION");
                        return true;
                    } 
                    collision = true;
                    Debug.WriteLine("Collision: " + collidee.Span.LowerBound + " | " + collidee.Span.UpperBound);
                    return false;
                }, ref target);


                if (!collision)
                {
                    quadtree.RemoveNode(element);
                    element.Span = target;
                    quadtree.AddNode(element);
                }
            }

            if (!collision)
            {
                position.position += velocity.velocity * (state.ElapsedGameTime.Milliseconds / 1000f);
            }
        }
    }
}

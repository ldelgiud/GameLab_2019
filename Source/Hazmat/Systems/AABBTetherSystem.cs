using System;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class AABBTetherSystem : AEntitySystem<Time>
    {
        QuadTree<Entity> quadtree;

        public AABBTetherSystem(World world) : base(
            world.GetEntities()
            .With<AABBComponent>()
            .With<AABBTetherComponent>()
            .Build()
            )
        {
            this.quadtree = Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var tether = ref entity.Get<AABBTetherComponent>();
            ref var aabb = ref entity.Get<AABBComponent>();

            if (tether.parent.Has<AABBComponent>())
            {
                ref var parentAABB = ref tether.parent.Get<AABBComponent>();

                // Remove node from quatree
                quadtree.RemoveNode(aabb.element);

                // Update center to match parent box
                var lowerBound = aabb.element.Span.LowerBound - aabb.element.Span.Center;
                var upperBound = aabb.element.Span.UpperBound - aabb.element.Span.Center;
                aabb.element.Span.LowerBound = lowerBound + parentAABB.element.Span.Center;
                aabb.element.Span.UpperBound = upperBound + parentAABB.element.Span.Center;

                // Insert back into quadtree
                quadtree.AddNode(aabb.element);
            }
        }
    }
}

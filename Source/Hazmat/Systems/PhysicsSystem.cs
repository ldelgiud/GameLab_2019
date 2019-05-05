using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    sealed class PhysicsSystem : AEntitySystem<Time>
    {
        public QuadTree<Entity> quadtree;
        ICollisionSet collisionSet;

        public PhysicsSystem(World world, QuadTree<Entity> quadtree, ICollisionSet collisionSet) : base(
            world.GetEntities()
            .With<Transform3DComponent>()
            .With<WorldSpaceComponent>()
            .With<VelocityComponent>()
            .Build()) {
            this.quadtree = quadtree;
            this.collisionSet = collisionSet;
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();
            ref NameComponent name = ref entity.Get<NameComponent>();

            bool collision = false;
            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                Element<Entity> element = aabb.element;

                AABB target = new AABB {
                    LowerBound = element.Span.LowerBound + velocity.velocity * time.Delta,
                    UpperBound = element.Span.UpperBound + velocity.velocity * time.Delta
                };

                bool solid = aabb.solid;
                List<Entity> collisions = new List<Entity>();
                this.quadtree.QueryAABB((Element<Entity> collidee) =>
                {
                    AABBComponent collideeAABB = collidee.Value.Get<AABBComponent>();
                    if (collidee == element)
                    {
                    }
                    else if(!solid || !collideeAABB.solid)
                    {
                        collisions.Add(collidee.Value);
                    }
                    else
                    {
                        collisions.Add(collidee.Value);
                        collision = true;
                    }
                    return true;
                    
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
                transform.value.LocalTranslation += (velocity.velocity * time.Delta).ToVector3();
            }
        }
    }
}

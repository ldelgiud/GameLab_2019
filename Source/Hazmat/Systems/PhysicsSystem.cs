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

            int sliding = 1;
            bool moveMe; 
            bool collision = false;
            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                Element<Entity> element = aabb.element;
                Vector2 movement = velocity.velocity * time.Delta;
                AABB target = new AABB {
                    LowerBound = element.Span.LowerBound + movement,
                    UpperBound = element.Span.UpperBound + movement
                };
                AABB testVert = new AABB
                {
                    LowerBound = element.Span.LowerBound + new Vector2(0, movement.Y),
                    UpperBound = element.Span.UpperBound + new Vector2(0, movement.Y)
                };
                AABB testHor = new AABB
                {
                    LowerBound = element.Span.LowerBound + new Vector2(movement.X, 0),
                    UpperBound = element.Span.UpperBound + new Vector2(movement.X, 0)
                };
                bool solid = aabb.solid;
                List<Entity> collisions = new List<Entity>();
                List<AABB> hardCollisions = new List<AABB>();
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
                        hardCollisions.Add(collidee.Span);
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
                else
                {
                    foreach(var solidHit in hardCollisions)
                    {
                        AABB hitAABB = solidHit;
                        if (!AABB.TestOverlap(ref testHor, ref hitAABB))
                        {
                            sliding *= 2;
                            target = testHor;
                            velocity.velocity = new Vector2(velocity.velocity.X, 0);
                            
                        }
                        else if (!AABB.TestOverlap(ref testVert, ref hitAABB))
                        {
                            sliding *= 3;
                            target = testVert;
                            velocity.velocity = new Vector2(0, velocity.velocity.Y);
                            
                        }
                    }
                    if ((sliding % 2 == 0) && !(sliding % 6 == 0))
                    {
                        quadtree.RemoveNode(element);
                        element.Span = testHor;
                        quadtree.AddNode(element);
                    } else if ((sliding % 3 == 0) && !(sliding % 6 == 0))
                    {
                        quadtree.RemoveNode(element);
                        element.Span = testVert;
                        quadtree.AddNode(element);
                    }
                }
            }
            moveMe = ((sliding % 2 == 0) || (sliding % 3 == 0)) && !(sliding % 6 == 0);
            if (!collision || moveMe)
            {
                transform.value.LocalTranslation += (velocity.velocity * time.Delta).ToVector3();
            } 
        }
    }
}

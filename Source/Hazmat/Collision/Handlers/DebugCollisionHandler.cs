using System;
using System.Diagnostics;

using DefaultEcs;

using Hazmat.Components;

namespace Hazmat.Collision.Handlers
{
    /// <summary>
    /// Debug collision handler that prints the collision type and participants
    /// </summary>
    class DebugCollisionHandler : CollisionHandler
    {
        public DebugCollisionHandler(World world) : base(
            new Type[] { typeof(NameComponent), typeof(AABBComponent) },
            new Type[] { typeof(NameComponent), typeof(AABBComponent) }
            ) { }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {


            Debug.WriteLine(String.Format(
                "Collision!\n" +
                "Entity {0}: {1} {2}\n" +
                "Entity {3}: {4} {5}",
                collider.Get<NameComponent>().name,
                collider.Get<AABBComponent>().element.Span.LowerBound,
                collider.Get<AABBComponent>().element.Span.UpperBound,
                collidee.Get<NameComponent>().name,
                collidee.Get<AABBComponent>().element.Span.LowerBound,
                collidee.Get<AABBComponent>().element.Span.UpperBound
                ));
        }
    }
}

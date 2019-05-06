using System;

using DefaultEcs;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Collision.Handlers
{
    class DamageSolidCollisionHandler : CollisionHandler
    {
        public DamageSolidCollisionHandler(World world) : base(
            new Type[] { typeof(DamageComponent), typeof(AABBComponent), typeof(AllianceMaskComponent) },
            new Type[] { typeof(AABBComponent) },
            true
            )
        {
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            // Only handle solid non-damage components
            if (collidee.Has<AllianceMaskComponent>() || !collidee.Get<AABBComponent>().solid) return;

            switch (type)
            {
                case CollisionType.Start:
                    collider.Delete();
                    break;
            }
        }
    }
}

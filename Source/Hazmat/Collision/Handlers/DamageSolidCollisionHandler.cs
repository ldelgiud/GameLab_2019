using System;

using DefaultEcs;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Microsoft.Xna.Framework;

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
            if (collider.Has<AIComponent>()) return;
            switch (type)
            {
                case CollisionType.Start:
                    Vector3 colliderPos = collider.Get<Transform3DComponent>().value.Translation;
                    DamageComponent damage = collider.Get<DamageComponent>();

                    SpawnHelper.SpawnExplosion(colliderPos.ToVector2(), damage.animationPath, damage.skinPath);

                    collider.Delete();
                    break;
            }
        }
    }
}

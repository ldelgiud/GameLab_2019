using System;
using System.Diagnostics;

using DefaultEcs;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Microsoft.Xna.Framework;

namespace Hazmat.Collision.Handlers
{
    class DamageHealthCollisionHandler : CollisionHandler
    {
        public DamageHealthCollisionHandler(World world) : base(
            new Type[] { typeof(DamageComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            new Type[] { typeof(HealthComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)}
            )
        { }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            Alliance colliderAlliance = collider.Get<AllianceMaskComponent>().alliance;
            Alliance collideeAlliance = collidee.Get<AllianceMaskComponent>().alliance;
            if (colliderAlliance == collideeAlliance) return;
            HealthComponent health = collidee.Get<HealthComponent>();
            DamageComponent damage = collider.Get<DamageComponent>();
            Vector2 collideePos = collidee.Get<Transform2DComponent>().value.Translation;

            health.DealDamage(damage.Damage);
            if (health.isDead())
            {
                bool drop = Constants.RANDOM.NextDouble() < HelperFunctions.DropRate();
                if (drop) SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, collideePos);
                collidee.Delete();
            }

            collider.Delete();
        }
    }
}

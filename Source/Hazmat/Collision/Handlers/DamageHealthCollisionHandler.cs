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
        Score score;

        public DamageHealthCollisionHandler(World world, Score score) : base(
            new Type[] { typeof(DamageComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            new Type[] { typeof(HealthComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)}
            )
        {
            this.score = score;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            Alliance colliderAlliance = collider.Get<AllianceMaskComponent>().alliance;
            Alliance collideeAlliance = collidee.Get<AllianceMaskComponent>().alliance;
            if (colliderAlliance == collideeAlliance) return;
            HealthComponent health = collidee.Get<HealthComponent>();
            DamageComponent damage = collider.Get<DamageComponent>();
            Vector3 collideePos = collidee.Get<Transform3DComponent>().value.Translation;

            health.DealDamage(damage.Damage);
            if (health.isDead())
            {
                this.score.Kills += 1;
                bool drop = Constants.RANDOM.NextDouble() < HelperFunctions.DropRate();
                if (drop) SpawnHelper.SpawnBattery(Constants.SMALL_BATTERY_SIZE, collideePos.ToVector2());
                collidee.Delete();
            }

            collider.Delete();
        }
    }
}

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

            // Apply blink effect
            float timeDamageEffect = 0.2f;
            if (collidee.Has<DamageEffectComponent>())
            {
                ref DamageEffectComponent damageEffect = ref collidee.Get<DamageEffectComponent>();
                damageEffect.Initialize(timeDamageEffect);
            }
            else
            {
                ref ModelComponent model = ref collidee.Get<ModelComponent>();
                model.EnableDamageEffect();
                DamageEffectComponent damageEffect = new DamageEffectComponent();
                damageEffect.Initialize(timeDamageEffect);
                collidee.Set(damageEffect);
            }

            health.DealDamage(damage.Damage);
            if (health.isDead())
            {
                this.score.Kills += 1;
                bool drop = Constants.RANDOM.NextDouble() < HelperFunctions.DropRate();
                if (drop)
                {
                    int size = Constants.RANDOM.Next(100);
                    uint batterySize;
                    if (size <= 60) batterySize = Constants.SMALL_BATTERY_SIZE;
                    else if (size < 90) batterySize = Constants.MEDIUM_BATTERY_SIZE;
                    else batterySize = Constants.BIG_BATTERY_SIZE;
                    SpawnHelper.SpawnBattery(batterySize, collideePos.ToVector2());

                }
                collidee.Delete();
            }

            collider.Delete();
        }
    }
}

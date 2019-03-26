using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Microsoft.Xna.Framework;

namespace Meltdown.Collision.Handlers
{
    class ProjectileCollisionHandler : CollisionHandler
    {
        public ProjectileCollisionHandler(World world) : base(
            world.GetEntities()
            .With<ProjectileComponent>()
            .With<AABBComponent>()
            .Build(),
            world.GetEntities()
            .With<HealthComponent>()
            .With<AABBComponent>()
            .Build()
            )
        { }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            ref HealthComponent health = ref collidee.Get<HealthComponent>();
            ref ProjectileComponent projectile = ref collider.Get<ProjectileComponent>();
            Vector2 collideePos = collidee.Get<AABBComponent>().aabb.Center;
            health.DealDamage(projectile.damage);

            if (health.isDead())
            {
                bool drop = Constants.RANDOM.Next(5) == 1;
                if (drop) SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, collideePos);
                collidee.Delete();

            }

            collider.Delete();
        }


    }
}

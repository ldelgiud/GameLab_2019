using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
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

            health.DealDamage(projectile.damage);

            if (health.isDead())
            {
                collidee.Delete();
            }
            
            collider.Delete();
        }


    }
}

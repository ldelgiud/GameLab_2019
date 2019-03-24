using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision.Handlers
{
    class EnergyPickupCollisionHandler : CollisionHandler
    {
        public EnergyPickupCollisionHandler(World world) : base(
            world.GetEntities()
            .With<PlayerComponent>()
            .With<AABBComponent>()
            .Build(),
            world.GetEntities()
            .With<EnergyPickupComponent>()
            .With<AABBComponent>()
            .Build()
            )
        {
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            collidee.Delete();
        }
    }
}

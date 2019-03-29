using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision.Handlers
{
    class EnergyPickupCollisionHandler : CollisionHandler
    {
        Energy energy;

        public EnergyPickupCollisionHandler(World world, Energy energy) : base(
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
            this.energy = energy;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            energy.CurrentEnergy += collidee.Get<EnergyPickupComponent>().value;
            collidee.Delete();
        }
    }
}

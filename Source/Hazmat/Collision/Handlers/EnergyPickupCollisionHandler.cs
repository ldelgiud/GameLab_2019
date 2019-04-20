using System;
using System.Diagnostics;

using DefaultEcs;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Collision.Handlers
{
    class EnergyPickupCollisionHandler : CollisionHandler
    {
        Energy energy;

        public EnergyPickupCollisionHandler(World world, Energy energy) : base(
            new Type[] { typeof(PlayerComponent), typeof(AABBComponent)},
            new Type[] { typeof(EnergyPickupComponent), typeof(AABBComponent)}
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

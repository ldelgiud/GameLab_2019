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
        Score score;

        public EnergyPickupCollisionHandler(World world, Energy energy, Score score) : base(
            new Type[] { typeof(PlayerComponent), typeof(AABBComponent)},
            new Type[] { typeof(EnergyPickupComponent), typeof(AABBComponent)}
            )
        {
            this.energy = energy;
            this.score = score;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            this.score.Batteries += 1;
            energy.CurrentEnergy += collidee.Get<EnergyPickupComponent>().value;
            collidee.Delete();
        }
    }
}

using System;
using System.Diagnostics;

using DefaultEcs;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Collision.Handlers
{
    class PlayerDamageCollisionHandler : CollisionHandler
    {
        Energy energy;

        public PlayerDamageCollisionHandler(World world, Energy energy) : base(
            new Type[] {typeof(PlayerComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            new Type[] {typeof(DamageComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            true
            )
        {
            this.energy = energy;
        }
        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            switch(type) {
                case CollisionType.Start:
                    Alliance playerAlliance = collider.Get<AllianceMaskComponent>().alliance;
                    Alliance collideeAlliance = collidee.Get<AllianceMaskComponent>().alliance;
                    if (((int)playerAlliance | (int)collideeAlliance) != (int)playerAlliance)
                    {
                        energy.CurrentEnergy -= collidee.Get<DamageComponent>().Damage;
                        collidee.Delete();
                    }
                    break;
            }

            
            
        }
    }
}

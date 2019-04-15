using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision.Handlers
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
            //TODO: change this check to a mask

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

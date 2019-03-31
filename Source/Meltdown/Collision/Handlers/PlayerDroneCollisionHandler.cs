﻿using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Collision;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision.Handlers
{
    class PlayerDroneCollisionHandler : CollisionHandler
    {
        Energy energy;

        public PlayerDroneCollisionHandler(World world, Energy energy) : base(
            new Type[] {typeof(PlayerComponent), typeof(AABBComponent)},
            new Type[] {typeof(DroneComponent), typeof(AABBComponent)},
            true
            )
        {
            this.energy = energy;
        }
        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            energy.CurrentEnergy -= collidee.Get<DroneComponent>().Damage;
            collidee.Delete();
        }
    }
}

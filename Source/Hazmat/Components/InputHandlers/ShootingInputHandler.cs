using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Hazmat.Input;
using Hazmat.Utilities;
using Hazmat.Graphics;
using Hazmat.Utilities.Extensions;
using Hazmat.Components.InputHandlers;
using Hazmat.Music;

namespace Hazmat.Components.InputHandlers
{
    class ShootingInputHandler : IInputHandler
    {
        World world;
        StatsComponent playerStats;


        public ShootingInputHandler(World world, StatsComponent playerStats)
        {
            this.world = world;
            this.playerStats = playerStats;
        }

        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {

            ref SmallGunComponent smallGun = ref entity.Get<SmallGunComponent>();
            ref Transform3DComponent gunTransform = ref entity.Get<Transform3DComponent>();

            smallGun.additionalDamage = playerStats.Damage;
            
            Vector2 direction = new Vector2(MathF.Cos(gunTransform.value.Rotation.Z), MathF.Sin(gunTransform.value.Rotation.Z));

            switch (inputManager.GetEvent(0, ThumbSticks.Right))
            {
                case ValueEvent<Vector2> value:
                    if (value.current.LengthSquared() != 0)
                    {
                        smallGun.Shoot(time.Absolute, gunTransform.value, Camera2D.PerspectiveToWorld(value.current));
                    }
                    
                    break;
            }

            switch (inputManager.GetEvent(Keys.F))
            {
                case ReleaseEvent _: break;
                case HoldEvent _: break;
                case PressEvent _:
                    smallGun.Shoot(time.Absolute, gunTransform.value,direction); 
                    break;
            }
        }
    }
}

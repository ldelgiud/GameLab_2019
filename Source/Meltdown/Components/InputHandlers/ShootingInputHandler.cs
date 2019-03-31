using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Meltdown.Input;
using Meltdown.Utilities;


namespace Meltdown.Components.InputHandlers
{
    class ShootingInputHandler : IInputHandler
    {
        World world;

        public ShootingInputHandler(World world)
        {
            this.world = world;
        }

        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {

            ref SmallGunComponent smallGun = ref entity.Get<SmallGunComponent>();
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();

            switch (inputManager.GetEvent(Keys.F))
            {
                case ReleaseEvent _: break;
                case HoldEvent _: break;
                case PressEvent _:
                    smallGun.Shoot(time.Absolute, transform, new Vector2(1, 0), world);
                    break;
            }
        }
    }
}

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

namespace Hazmat.Components.InputHandlers
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
            ref Transform2DComponent gunTransform = ref entity.Get<Transform2DComponent>();

            Vector2 direction = gunTransform.value.Rotation.ToVector2();
            direction = Camera2D.PerspectiveToWorld(direction);

            switch (inputManager.GetEvent(0, Buttons.RightTrigger))
            {
                case HoldEvent _: 
                case PressEvent _:
                    smallGun.Shoot(time.Absolute, gunTransform.value, direction);
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

using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Meltdown.Input;
using Meltdown.Utilities;
using Meltdown.Graphics;

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
            ref WorldTransformComponent gunTransform = ref entity.Get<WorldTransformComponent>();

            MouseState mState = Mouse.GetState();
            Vector2 direction = (mState.Position.ToVector2() - Game1.Instance.Window.ClientBounds.Center.ToVector2()) * new Vector2(1, -1);
            direction.Normalize();

            switch (inputManager.GetEvent(Keys.F))
            {
                case ReleaseEvent _: break;
                case HoldEvent _: break;
                case PressEvent _:
                    smallGun.Shoot(time.Absolute, gunTransform, direction, world); 
                    break;
            }
        }
    }
}

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
            ref Transform2DComponent gunTransform = ref entity.Get<Transform2DComponent>();

            MouseState mState = Mouse.GetState();
            Vector2 direction = (mState.Position.ToVector2() - Game1.Instance.Window.ClientBounds.Center.ToVector2()) * new Vector2(1, -1);

           switch(inputManager.GetEvent(0, ThumbSticks.Right))
           {
                case ValueEvent<Vector2> v:
                    direction = v.current;
                    if (direction == Vector2.Zero)
                    {
                        direction = Vector2.UnitX;
                    }
                    break;
           }

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
                    smallGun.Shoot(time.Absolute, gunTransform.value, direction); 
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Meltdown.Input;
using Meltdown.Graphics;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Components.InputHandlers
{
    class PlayerInputHandler : IInputHandler
    {
        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            ref VelocityComponent velComp = ref entity.Get<VelocityComponent>();
            ref PlayerComponent player = ref entity.Get<PlayerComponent>();
            ref Transform2DComponent transform = ref entity.Get<Transform2DComponent>();

            velComp.velocity = Vector2.Zero;

            // KeyBoard
            switch (inputManager.GetEvent(Keys.Left))
            {
                case ReleaseEvent _: break;
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Right))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Up))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Down))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -player.Speed;
                    break;
            }

            // KeyBoard - WASD
            switch (inputManager.GetEvent(Keys.A))
            {
                case ReleaseEvent _: break;
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.D))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.W))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.S))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -player.Speed;
                    break;
            }


            // GamePad
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickLeft))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickRight))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickUp))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = player.Speed;
                break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickDown))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -player.Speed;
                    break;
            }

            if (velComp.velocity != Vector2.Zero)
            {
                transform.value.Rotation = velComp.velocity.ToRotation();
                velComp.velocity = Camera2D.PerspectiveToWorld(velComp.velocity);
                velComp.velocity.Normalize();
                velComp.velocity *= player.Speed;
            }
            

            //if (gState.IsButtonDown(Buttons.LeftTrigger)) //L2
            //{
            //    //Debug.WriteLine("Trigger");
            //    //Shoot
            //}

            //if (gState.IsButtonDown(Buttons.LeftShoulder)) //L1
            //{
            //    Debug.WriteLine("Shoulder");
            //}

        }
    }
}

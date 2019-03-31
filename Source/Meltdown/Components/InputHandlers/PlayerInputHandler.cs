﻿using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Meltdown.Input;
using Meltdown.Utilities;

namespace Meltdown.Components.InputHandlers
{
    class PlayerInputHandler : IInputHandler
    {
        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            ref VelocityComponent velComp = ref entity.Get<VelocityComponent>();
            velComp.velocity = Vector2.Zero;

            // KeyBoard
            switch (inputManager.GetEvent(Keys.Left))
            {
                case ReleaseEvent _: break;
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -5;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Right))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = 5;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Up))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = 5;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Down))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -5;
                    break;
            }

            // GamePad
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickLeft))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -5;
                    break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickRight))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = 5;
                    break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickUp))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = 5;
                break;
            }
            switch (inputManager.GetEvent(0, Buttons.LeftThumbstickDown))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -5;
                    break;
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
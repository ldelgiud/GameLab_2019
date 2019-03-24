using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Meltdown.Components.InputHandlers
{
    class InputHandlerPlayer : IInputHandler
    {

        Entity e;
        public InputHandlerPlayer(Entity e)
        {
            this.e = e;
        }

        public void HandleInput()
        {

            KeyboardState kState = Keyboard.GetState();
            GamePadState gState = GamePad.GetState(PlayerIndex.One);

            ref VelocityComponent velComp = ref e.Get<VelocityComponent>();
            velComp.velocity = Vector2.Zero;

            if (gState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                velComp.velocity.X = -100;
            }

            if (gState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                velComp.velocity.X = 100;
            }

            if (gState.IsButtonDown(Buttons.LeftThumbstickUp))
            {
                velComp.velocity.Y = 100;
            }

            if (gState.IsButtonDown(Buttons.LeftThumbstickDown))
            {
                velComp.velocity.Y = -100;
            }

            if (gState.IsButtonDown(Buttons.LeftTrigger)) //L2
            {
                //Debug.WriteLine("Trigger");
                //Shoot
            }

            if (gState.IsButtonDown(Buttons.LeftShoulder)) //L1
            {
                Debug.WriteLine("Shoulder");
            }


            ////Dpad
            //if (gState.IsButtonDown(Buttons.DPadRight))
            //{
            //    velComp.velocity.X = 100;
            //}
            //if (gState.IsButtonDown(Buttons.DPadLeft))
            //{
            //    velComp.velocity.X = -100;
            //}
            //if (gState.IsButtonDown(Buttons.DPadDown))
            //{
            //    velComp.velocity.Y = 100;
            //}
            //if (gState.IsButtonDown(Buttons.DPadUp))
            //{
            //    velComp.velocity.Y = -100;
            //}

            // Keyboard test
            if (kState.IsKeyDown(Keys.Right))
            {
                velComp.velocity.X = 100;
            }
            if (kState.IsKeyDown(Keys.Left))
            {
                velComp.velocity.X = -100;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                velComp.velocity.Y = -100;
            }
            if (kState.IsKeyDown(Keys.Up))
            {
                velComp.velocity.Y = 100;
            }
            
            // Keyboard WASD test
            if (kState.IsKeyDown(Keys.D))
            {
                velComp.velocity.X = 100;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                velComp.velocity.X = -100;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                velComp.velocity.Y = -100;
            }
            if (kState.IsKeyDown(Keys.W))
            {
                velComp.velocity.Y = 100;
            }

        }
    }
}

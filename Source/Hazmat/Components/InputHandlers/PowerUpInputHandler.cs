﻿using System;
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
    class PowerUpInputHandler : IInputHandler
    {

        World world;

        public PowerUpInputHandler(World world)
        {
            this.world = world;
        }

        /* Assumption: a power up object when instantiated needs to have
                       DisplayPowerUpChoiceComponent, InputComponent and a StatsComponent from the player.
                       See PowerUpPickUpCollisionHandler.
        */
        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            // Need to remove in case a button is pressed.
            // ref DisplayPowerUpChoiceComponent displayPowerUp = ref entity.Get<DisplayPowerUpChoiceComponent>();

            // StatsComponent of the player need for upgrades in case button is pressed.
            ref StatsComponent stats = ref entity.Get<StatsComponent>();


            // Choose between left and right power up
            switch (inputManager.GetEvent(Keys.R))
            {
                case HoldEvent _:
                case PressEvent _:
                    // TODO: add logic for choosing right
                    stats.UpgradeSpeed(30);
                    entity.Delete();
                    break;
            }

            switch (inputManager.GetEvent(Keys.T))
            {
                case HoldEvent _:
                case PressEvent _:
                    // TODO: add logic for choosing right
                    stats.UpgradeDamage(30);
                    entity.Delete();
                    break;
            }

            // GAMEPAD
            switch (inputManager.GetEvent(0, Buttons.RightShoulder))
            {
                case HoldEvent _:
                case PressEvent _:
                    // TODO: add logic for choosing right
                    break;
            }

            switch (inputManager.GetEvent(0, Buttons.LeftShoulder))
            {
                case HoldEvent _:
                case PressEvent _:
                    // TODO: add logic for choosing left
                    break;
            }
        }

    }
}
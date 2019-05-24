using System;
using System.Collections.Generic;

using DefaultEcs;
using DefaultEcs.Resource;
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
        Score score;
        World world;

        public PowerUpInputHandler(World world, Score score)
        {
            this.world = world;
            this.score = score;
        }

        /* Assumption: a power up object when instantiated needs to have
                       DisplayPowerUpChoiceComponent, InputComponent and a StatsComponent from the player.
                       See PowerUpPickUpCollisionHandler.
        */
        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            // Need to remove in case a button is pressed.
            ref DisplayPowerUpChoiceComponent displayPowerUp = ref entity.Get<DisplayPowerUpChoiceComponent>();

            // StatsComponent of the player need for upgrades in case button is pressed.
            ref StatsComponent stats = ref entity.Get<StatsComponent>();

            if (!displayPowerUp.UpgradeAlreadyChoosen)
            {

                var armorInputEvent = inputManager.GetEvent(Keys.R) ?? inputManager.GetEvent(0, Buttons.LeftShoulder);
                var weaponInputEvent = inputManager.GetEvent(Keys.T) ?? inputManager.GetEvent(0, Buttons.RightShoulder);

                // Choose between left and right power up
                switch (armorInputEvent)
                {
                    case PressEvent _:
                        this.score.ArmorUpgrades += 1;
                        stats.UpgradeDefense();

                        entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                               new SpineAnimationInfo(
                                   @"ui\SPS_Screens",
                                   new SkeletonInfo(10f, 7f, skin: "upgrade_popup"),
                                   new AnimationStateInfo("upgrade_left", false)
                               )
                           ));

                        Hazmat.Instance.SoundManager.PlaySoundEffect(Hazmat.Instance.SoundManager.PowerUp);


                        stats.CurrentlyDisplayingOtherPowerUp = false;
                        displayPowerUp.DisplayForChoiceAnimation();
                        displayPowerUp.UpgradeAlreadyChoosen = true;
                        break;
                }

                switch (weaponInputEvent)
                {
                    case PressEvent _:
                        this.score.WeaponUpgrades += 1;
                        stats.UpgradeDamage(Constants.DAMAGE_UPGRADE);

                        entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                               new SpineAnimationInfo(
                                   @"ui\SPS_Screens",
                                   new SkeletonInfo(10f, 7f, skin: "upgrade_popup"),
                                   new AnimationStateInfo("upgrade_right", false)
                               )
                           ));

                        Hazmat.Instance.SoundManager.PlaySoundEffect(Hazmat.Instance.SoundManager.PowerUp);

                        stats.CurrentlyDisplayingOtherPowerUp = false;
                        displayPowerUp.DisplayForChoiceAnimation();
                        displayPowerUp.UpgradeAlreadyChoosen = true;
                        break;
                }
            }
        }

    }
}

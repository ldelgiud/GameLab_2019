using System;

using DefaultEcs;
using Hazmat.Graphics;

using Microsoft.Xna.Framework;

namespace Hazmat.Components
{
    class DisplayPowerUpChoiceComponent
    {
        private StatsComponent PlayerStats;

        public bool UpgradeAlreadyChoosen { get; set; }

        /// <summary>
        /// Time specifying for how long the power up choice will be displayed
        /// </summary>
        public float ShowTime { get; private set; }

        /// <summary>
        /// Time specifying for how long the power up choice has been displayed
        /// </summary>
        public float CurrentShowTime { get; private set; }


        public DisplayPowerUpChoiceComponent(float time, StatsComponent playerStats)
        {
            this.ShowTime = time;
            this.CurrentShowTime = 0;
            this.PlayerStats = playerStats;
        }

        public void IncrementCurrentShowTime(float amount)
        {
            this.CurrentShowTime += amount;
        }

        /// <summary>
        /// When a button has been displayed play the animation for choice, so it should 
        /// be displaied for a second. See PowerUpInputHandler.
        /// </summary>
        public void DisplayForChoiceAnimation()
        {
                this.ShowTime = 1f;
                this.CurrentShowTime = 0f;
        }

        public void EndDisplay()
        {
            this.PlayerStats.CurrentlyDisplayingOtherPowerUp = false;
        }

    }
}

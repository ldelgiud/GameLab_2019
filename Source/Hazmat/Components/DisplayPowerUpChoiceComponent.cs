using System;

using DefaultEcs;
using Hazmat.Graphics;

using Microsoft.Xna.Framework;

namespace Hazmat.Components
{
    class DisplayPowerUpChoiceComponent
    {
        /// <summary>
        /// Time specifying for how lonf the power up choice will be displayed
        /// </summary>
        public float ShowTime { get; }

        /// <summary>
        /// Time specifying for how lonf the power up choice has been displayed
        /// </summary>
        public float CurrentShowTime { get; private set; }


        public DisplayPowerUpChoiceComponent(float time)
        {
            this.ShowTime = time;
            this.CurrentShowTime = 0;
        }

        public void IncrementCurrentShowTime(float amount)
        {
            this.CurrentShowTime += amount;
        }


    }
}

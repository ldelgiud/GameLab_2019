using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using Meltdown.Graphics;

using Microsoft.Xna.Framework;

namespace Meltdown.Components
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

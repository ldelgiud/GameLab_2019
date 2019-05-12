using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Utilities
{
    class Energy
    {
        public bool Active = true;

        private double currentEnergy;
        public double CurrentEnergy
        {
            get
            {
                return this.currentEnergy;
            }

            set
            {
                if (this.Active)
                {
                    this.currentEnergy = Math.Max(0, Math.Min(value, Constants.PLAYER_INITIAL_ENERGY));
                }
            }
        }

        /// <summary>
        /// Generate new Energy object with maximum energy
        /// </summary>
        public Energy()
        {
            this.CurrentEnergy = Constants.PLAYER_INITIAL_ENERGY;
        }
    }
}


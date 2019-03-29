using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Utilities
{
    class Energy
    {
        private double currentEnergy;
        public double CurrentEnergy
        {
            get
            {
                return this.currentEnergy;
            }

            set
            {
                this.currentEnergy = Math.Max(0, Math.Min(value, Constants.MAX_ENERGY));
            }
        }

        /// <summary>
        /// Generate new Energy object with maximum energy
        /// </summary>
        public Energy()
        {
            this.CurrentEnergy = Constants.MAX_ENERGY;
        }
    }
}


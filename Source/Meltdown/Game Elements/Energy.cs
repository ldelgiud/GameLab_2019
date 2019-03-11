using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown
{
    class Energy
    {
        private double currentEnergy; 
        public double CurrentEnergy { get {
                return this.currentEnergy;
            }

            set {
                this.currentEnergy = Math.Min(value, Energy.maxEnergy); 
            }
        }
        public const double maxEnergy = 1000;

        /// <summary>
        /// Generate new Energy object with maximum energy
        /// </summary>
        public Energy()
        {
            this.CurrentEnergy = maxEnergy;
        }
    }
}


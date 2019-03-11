using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown
{
    class Energy
    {
        public double energy { get; set; }
        public double maxEnergy;

        public Energy(double energy)
        {
            this.energy = energy;
            maxEnergy = energy;
        }
    }


}


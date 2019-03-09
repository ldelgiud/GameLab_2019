using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    class EnergyComponent
    {
        public float Energy { get; private set; }

        public EnergyComponent(float energy)
        {
            this.Energy = energy;
        }
    }
}

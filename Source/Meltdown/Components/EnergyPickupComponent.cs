﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    public struct EnergyPickupComponent
    {
        public uint value;

        public EnergyPickupComponent(uint value)
        {
            this.value = value;
        }
    }
}
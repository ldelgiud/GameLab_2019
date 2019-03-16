using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nez;

namespace Meltdown.Components
{
    class BatteryComponent : Component
    {
        public int Size { get; }
        
        /// <summary>
        /// Creates the battery component
        /// </summary>
        /// <param name="size">Amount of life restored if looted</param>
        public BatteryComponent(int size)
        {
            this.Size = size;
        }
    }
}

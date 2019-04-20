using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    class TTLComponent
    {
        float TTL;

        /// <summary>
        /// TTL given in Seconds
        /// </summary>
        /// <param name="TTL"></param>
        public TTLComponent(float TTL)
        {
            this.TTL = TTL;
        }

        /// <summary>
        /// decrases TTL by delta 
        /// </summary>
        /// <returns>true if TTL < 0 </returns>
        public bool decrease(float delta)
        {
            this.TTL -= delta;
            if (this.TTL - delta <= 0)
            {
                return true;
            }

            return false;
        }
    }
}

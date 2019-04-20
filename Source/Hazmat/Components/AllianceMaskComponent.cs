using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    [Flags]
    enum Alliance
    {
        Player = 1 << 0,
        Hostile = 1 << 1
    }

    class AllianceMaskComponent
    {
        public Alliance alliance;
        public AllianceMaskComponent(Alliance alliance)
        {
            this.alliance = alliance;
        }
    }
}

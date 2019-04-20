using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hazmat.AI;

namespace Hazmat.Components
{
    class AIComponent
    {
        public AIState State { get; set; }

        public AIComponent(AIState state)
        {
            this.State = state;
        }
    }
}

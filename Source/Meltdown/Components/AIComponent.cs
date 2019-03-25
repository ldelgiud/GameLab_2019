using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.AI;

namespace Meltdown.Components
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

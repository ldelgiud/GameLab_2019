using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Meltdown.AI
{
    abstract class AIState
    {
        abstract public void DoAction();

        abstract public AIState ChangeState();
    }
}

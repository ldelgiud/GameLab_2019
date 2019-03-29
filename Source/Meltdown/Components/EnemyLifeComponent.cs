using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    class EnemyLifeComponent
    {
        public float Life { get; private set; }
        public EnemyLifeComponent(float Life)
        {
            this.Life = Life;
        }
    }
}

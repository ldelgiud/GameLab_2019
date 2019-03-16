using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nez;

namespace Meltdown.Components
{
    class EnemyLifeComponent
    {
        public float MaxLife { get; private set; }
        public float Life { get; set; }

        /// <summary>
        /// Constructs object by setting life and MaxLife to the same value 
        /// </summary>
        /// <param name="life">value to set Life and MaxLife</param>
        public EnemyLifeComponent(float life)
        {
            this.Life = life;
            this.MaxLife = life;
        }
    }
}

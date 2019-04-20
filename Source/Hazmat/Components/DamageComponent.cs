using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    class DamageComponent
    {
        public float Damage { get; }

        public DamageComponent(float damage)
        {
            this.Damage = damage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    // Used for entities such as enemies etc.
    class HealthComponent
    {
        public float TotalHealth { get; private set; }
        public float Health { get; private set; }

        public HealthComponent(float health)
        {
            this.Health = health;
            this.TotalHealth = health;
        }

        public void DealDamage(float damage)
        {
            this.Health -= damage;
            if (this.Health < 0) this.Health = 0;
        }

        public bool isDead()
        {
            return this.Health == 0;
        }

    }
}

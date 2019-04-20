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

        float health;

        public HealthComponent(float health)
        {
            this.health = health;
        }

        public void DealDamage(float damage)
        {
            this.health -= damage;
            if (this.health < 0) this.health = 0;
        }

        public bool isDead()
        {
            return this.health == 0;
        }

    }
}

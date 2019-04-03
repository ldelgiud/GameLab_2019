using DefaultEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Components
{
    class WeaponComponent
    {
        public Entity weapon;

        public WeaponComponent(Entity entity)
        {
            this.weapon = entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    class DamageComponent
    {
        public string animationPath;
        public string skinPath;
        public float Damage { get; }

        public DamageComponent(float damage, string animationPath, string skinPath)
        {
            this.animationPath = animationPath;  
            this.Damage = damage;
            this.skinPath = skinPath;
        }
    }
}

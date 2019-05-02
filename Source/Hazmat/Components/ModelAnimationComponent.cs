using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tainicom.Aether.Animation;

namespace Hazmat.Components
{
    struct ModelAnimationComponent
    {
        public Animations animations;

        public ModelAnimationComponent(Animations animations)
        {
            this.animations = animations;
        }
    }
}

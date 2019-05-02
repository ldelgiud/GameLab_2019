using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spine;

namespace Hazmat.Components
{
    struct SpineAnimationComponent
    {
        public AnimationState value;

        public SpineAnimationComponent(AnimationState animationState)
        {
            this.value = animationState;
        }
    }
}

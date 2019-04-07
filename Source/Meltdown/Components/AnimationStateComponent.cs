using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spine;

namespace Meltdown.Components
{
    struct AnimationStateComponent
    {
        public AnimationState value;

        public AnimationStateComponent(AnimationState animationState)
        {
            this.value = animationState;
        }
    }
}

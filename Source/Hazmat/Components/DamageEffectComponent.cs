using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Components
{
    struct DamageEffectComponent
    {
        public float totalTimeEffect { get; private set; }
        public float currentTimeEffect { get; private set; }

        public void Initialize(float totalTimeEffect)
        {
            this.totalTimeEffect = totalTimeEffect;
            this.currentTimeEffect = 0f;
        }

        public void IncrementCurrentTimeEffect(float deltaTime)
        {
            this.currentTimeEffect += deltaTime;
        }
    }
}

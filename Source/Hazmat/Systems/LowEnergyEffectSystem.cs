using System;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using System.Diagnostics;

using Hazmat.PostProcessor;
using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class LowEnergyEffectSystem : AEntitySystem<Time>
    {

        Energy energy;
        PostProcessing postProcessing;

        float thresholdEnergy;
        bool redMode;

        float radiusXStart;
        float radiusYStart;

        float radiusXCurrent;
        float radiusYCurrent;

        float radiusXTarget = 0.4f;
        float radiusYTarget = 0.2f;

        float radiusAnimationTime = 3f;
        float radiusAnimationTimePassed = 0f;
        float animationPercent = 0;

        bool animationRound;

        public LowEnergyEffectSystem(World world, Energy energy, PostProcessing postProcessing) : base(
           world.GetEntities()
           .With<PlayerComponent>()
           .Build()
           )
        {
            this.energy = energy;
            this.postProcessing = postProcessing;

            //0.3f is the percentage
            this.thresholdEnergy = (float)Constants.PLAYER_INITIAL_ENERGY * 0.3f;

            // Set radius of vignette
            if (postProcessing.Effect.Parameters["radiusX"] != null)
            {
                radiusXStart = postProcessing.Effect.Parameters["radiusX"].GetValueSingle();
            }
            if (postProcessing.Effect.Parameters["radiusY"] != null)
            {
                radiusYStart = postProcessing.Effect.Parameters["radiusY"].GetValueSingle();
            }
        }

        protected override void Update(Time state, in Entity entity)
        {
            if (energy.CurrentEnergy < thresholdEnergy)
            {
                // Activate red mode
                if(!redMode && postProcessing.Effect.Parameters["redVignetteActive"] != null)
                {
                    postProcessing.Effect.Parameters["redVignetteActive"].SetValue(true);
                    redMode = true;
                }

                // Vignette Animation 
                radiusAnimationTimePassed += state.Delta;
                animationPercent = Math.Clamp(radiusAnimationTimePassed/radiusAnimationTime, 0, 1);
                radiusXCurrent = radiusXStart * (1 - animationPercent) + radiusXTarget * animationPercent;
                radiusYCurrent = radiusYStart * (1 - animationPercent) + radiusYTarget * animationPercent;

                postProcessing.Effect.Parameters["radiusX"].SetValue(radiusXCurrent);
                postProcessing.Effect.Parameters["radiusY"].SetValue(radiusYCurrent); 

                if (animationPercent == 1)
                {
                    ResetAnimationValues();
                }
            }
            else
            {
                if (redMode)
                {
                    postProcessing.Effect.Parameters["redVignetteActive"].SetValue(false);
                    redMode = false;
                    if (animationRound)
                    {
                        SwitchStartAndTarget();
                        animationRound = false;
                    }
                }
            }
        }

        public void SwitchStartAndTarget()
        {
            float temp = radiusXStart;
            radiusXStart = radiusXTarget;
            radiusXTarget = temp;

            temp = radiusYStart;
            radiusYStart = radiusYTarget;
            radiusYTarget = temp;
        }

        private void ResetAnimationValues()
        {
            SwitchStartAndTarget();

            animationPercent = 0f;
            radiusAnimationTimePassed = 0f;

            animationRound = !animationRound;
        }

    }
}

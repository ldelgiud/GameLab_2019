﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Spine;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class AnimationStateUpdateSystem : AEntitySystem<Time>
    {

        public AnimationStateUpdateSystem(World world) : base(
            world.GetEntities()
            .With<SpineAnimationComponent>()
            .With<SpineSkeletonComponent>()
            .Build()
            )
        {

        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var animationState = ref entity.Get<SpineAnimationComponent>();
            ref var skeleton = ref entity.Get<SpineSkeletonComponent>();

            animationState.value.Update(state.Delta);
            animationState.value.Apply(skeleton.value);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class SkeletonUpdateSystem : AEntitySystem<Time>
    {

        public SkeletonUpdateSystem(World world) : base(
            world.GetEntities()
            .With<AnimationStateComponent>()
            .With<SkeletonComponent>()
            .With<Transform2DComponent>()
            .Build()
            )
        {
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var animationState = ref entity.Get<AnimationStateComponent>();
            ref var skeleton = ref entity.Get<SkeletonComponent>();
            ref var transform = ref entity.Get<Transform2DComponent>();

            var translation = transform.value.Translation + skeleton.info.translation;
            var scale = transform.value.Scale * skeleton.info.scale;

            animationState.value.Apply(skeleton.value);
            skeleton.value.X = translation.X;
            skeleton.value.Y = translation.Y;
            skeleton.value.ScaleX = scale.X;
            skeleton.value.ScaleY = scale.Y;
            skeleton.value.UpdateWorldTransform();
        }

    }
}

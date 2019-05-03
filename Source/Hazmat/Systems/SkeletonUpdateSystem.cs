using System;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class SkeletonUpdateSystem : AEntitySystem<Time>
    {

        public SkeletonUpdateSystem(World world) : base(
            world.GetEntities()
            .With<SpineSkeletonComponent>()
            .Build()
            )
        {
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var skeleton = ref entity.Get<SpineSkeletonComponent>();
            skeleton.value.UpdateWorldTransform();
        }

    }
}

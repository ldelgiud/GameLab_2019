using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Spine;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class AnimationStateUpdateSystem : AEntitySystem<Time>
    {

        public AnimationStateUpdateSystem(World world) : base(
            world.GetEntities()
            .With<AnimationStateComponent>()
            .Build()
            )
        {

        }

        protected override void Update(Time state, in Entity entity)
        {
            entity.Get<AnimationStateComponent>().value.Update(state.Delta);
        }

    }
}

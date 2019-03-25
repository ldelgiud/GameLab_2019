using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class EventSystem : AEntitySystem<Time>
    {
        World world;

        public EventSystem(World world) : base(
            world.GetEntities()
            .With<EventComponent>()
            .Build()
            )
        {
            this.world = world;
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref EventComponent _event = ref entity.Get<EventComponent>();
            _event.value.Update(this.world);
        }
    }
}

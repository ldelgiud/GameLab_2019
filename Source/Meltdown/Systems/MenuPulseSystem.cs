using System;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class MenuPulseSystem : AEntitySystem<Time>
    {
        public MenuPulseSystem(World world) : base(
            world.GetEntities()
            .With<ScreenTransformComponent>()
            .With<MenuPulseComponent>()
            .Build()
            )
        {

        }

        protected override void Update(Time time, in Entity entity)
        {
            ref ScreenTransformComponent transform = ref entity.Get<ScreenTransformComponent>();
            ref MenuPulseComponent pulse = ref entity.Get<MenuPulseComponent>();

            // Change direction if scale above threshold
            if (pulse.scale >= 1.1 || pulse.scale <= 0.9)
            {
                pulse.grow = !pulse.grow;
            }

            // Set scale direction
            var scale = 1 + ((pulse.grow) ? pulse.speed : -pulse.speed) * time.Delta;
            pulse.scale *= scale;

            transform.Scale(scale, scale, 1);
        }
    }
}

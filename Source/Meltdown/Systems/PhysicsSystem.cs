using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;

namespace Meltdown.Systems
{
    sealed class PhysicsSystem : AEntitySystem<GameTime>
    {
        public PhysicsSystem(World world) : base(
            world.GetEntities()
            .With<PositionComponent>()
            .With<VelocityComponent>()
            .Build()) { }

        protected override void Update(GameTime state, in Entity entity)
        {
            ref PositionComponent position = ref entity.Get<PositionComponent>();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();

            position.x += velocity.dx * (state.ElapsedGameTime.Milliseconds / 1000f);
            position.y += velocity.dy * (state.ElapsedGameTime.Milliseconds / 1000f);
        }
    }
}

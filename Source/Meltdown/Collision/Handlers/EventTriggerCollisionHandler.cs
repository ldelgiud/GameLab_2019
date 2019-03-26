using System;

using DefaultEcs;

using Meltdown.Components;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision.Handlers
{
    class EventTriggerCollisionHandler : CollisionHandler
    {
        World world;

        public EventTriggerCollisionHandler(World world) : base(
            world.GetEntities()
            .With<PlayerComponent>()
            .Build(),
            world.GetEntities()
            .With<EventTriggerComponent>()
            .Build()
            )
        {
            this.world = world;
        }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            ref EventTriggerComponent eventTrigger = ref collidee.Get<EventTriggerComponent>();

            var entity = this.world.CreateEntity();

            var _event = eventTrigger._event;
            _event.Initialize(this.world, entity);
            entity.Set(new EventComponent(_event));

            collidee.Delete();
        }
    }
}

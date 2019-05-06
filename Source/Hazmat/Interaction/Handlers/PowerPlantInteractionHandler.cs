using System;

using DefaultEcs;

using Hazmat.Input;
using Hazmat.Event;
using Hazmat.Components;


namespace Hazmat.Interaction.Handlers
{
    class PowerPlantInteractionHandler : InteractionHandler
    {
        World world;

        public PowerPlantInteractionHandler(World world) : base(
            world.GetEntities()
            .With<PlayerComponent>()
            .Build(),
            world.GetEntities()
            .With<PowerPlantComponent>()
            .Build()
            )
        {
            this.world = world;
        }

        public override bool HandleInteraction(IInputEvent inputEvent, Entity interactor, Entity interactee)
        {

            switch (inputEvent)
            {
                case ReleaseEvent _:
                    {
                        var entity = this.world.CreateEntity();

                        var _event = new StoryConclusionEvent();
                        _event.Initialize(this.world, entity);
                        entity.Set(new EventComponent(_event));
                        entity.Set(new NameComponent() { name = "conclusion event" });
                    }
                    return true;
            }

            return false;
        }
    }
}

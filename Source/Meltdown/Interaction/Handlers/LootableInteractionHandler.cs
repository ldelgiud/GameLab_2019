using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Meltdown.Input;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Interaction.Handlers
{
    // ADD INTERACTION COMP
    // ADD CUSTOM COMP => used for handling interactionHandlers
    class LootableInteractionHandler : InteractionHandler
    {
        World world;

        public LootableInteractionHandler(World world) : base(
            world.GetEntities()
            .With<PlayerComponent>()
            .Build(),
            world.GetEntities()
            .With<LootableComponent>()
            .With<Transform2DComponent>()
            .With<WorldSpaceComponent>()
            .Build()
            )
        {
            this.world = world;
        }

        public override bool HandleInteraction(IInputEvent inputEvent, Entity interactor, Entity interactee)
        {
            switch (inputEvent)
            {
                // In case interaction happened do:
                case PressEvent _:
                    ref var transform = ref interactee.Get<Transform2DComponent>();

                    // Cannot interact anymore
                    interactee.Remove<InteractableComponent>();

                    // Graphical hint disappear
                    ref Texture2DComponent texture = ref interactee.Get<Texture2DComponent>();
                    texture.RemoveTemporaryEffect();

                    // Spawn battery
                    SpawnHelper.SpawnBattery(Constants.MEDIUM_BATTERY_SIZE, transform.value.Translation + new Vector2(-3,0));

                    return true;
            }

            return false;
        }
    }
}

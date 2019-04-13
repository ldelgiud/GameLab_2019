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
            .With<WorldTransformComponent>()
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
                    ref var transform = ref interactee.Get<WorldTransformComponent>();

                    // Cannot interact anymore
                    interactee.Remove<InteractableComponent>();

                    // Graphical hint disappear
                    ref TextureComponent texture = ref interactee.Get<TextureComponent>();
                    texture.glowing = false;

                    // Spawn battery
                    SpawnHelper.SpawnBattery(Constants.MEDIUM_BATTERY_SIZE, transform.value.position.ToVector2() + new Vector2(0, 10));

                    return true;
            }

            return false;
        }
    }
}

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
                case PressEvent _:
                    ref var transform = ref interactee.Get<WorldTransformComponent>();

                    // Spawn battery
                    interactee.Remove<InteractableComponent>();

                    // Remove glowing
                    ref TextureEffectComponent texture = ref interactee.Get<TextureEffectComponent>();
                    interactee.Set(new TextureComponent() { value = texture.value });
                    interactee.Remove<TextureEffectComponent>();

                    SpawnHelper.SpawnBattery(Constants.MEDIUM_BATTERY_SIZE, transform.value.position.ToVector2() + new Vector2(0, 10));

                    return true;
            }

            return false;
        }
    }
}

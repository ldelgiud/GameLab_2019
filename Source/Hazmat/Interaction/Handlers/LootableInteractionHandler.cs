using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Hazmat.Input;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Interaction.Handlers
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
            .With<Transform3DComponent>()
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
                    ref var transform = ref interactee.Get<Transform3DComponent>();
                    ref ModelComponent model = ref interactee.Get<ModelComponent>();

                    // Cannot interact anymore
                    interactee.Remove<InteractableComponent>();

                    // Graphical hint disappear
                    model.DisableToonGlow();

                    // Spawn battery
                    //Big Battery
                    SpawnHelper.SpawnBattery(
                        Constants.BIG_BATTERY_SIZE,
                        transform.value.Translation.ToVector2(),
                        Constants.BIG_BATTERY_SCALE
                        );

                    //Normal Batteries 
                    Vector2 disp =  new Vector2((float)Constants.RANDOM.NextDouble() * 2f, (float)Constants.RANDOM.NextDouble() * 2f);
                    SpawnHelper.SpawnBattery(
                        Constants.MEDIUM_BATTERY_SIZE,
                        transform.value.Translation.ToVector2() + disp
                        );
                    SpawnHelper.SpawnBattery(
                        Constants.MEDIUM_BATTERY_SIZE, 
                        transform.value.Translation.ToVector2() - disp
                        );


                    return true;
            }

            return false;
        }
    }
}

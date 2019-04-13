using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Meltdown.Input;
using Meltdown.Components;
using Meltdown.Components.InputHandlers;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Interaction.Handlers
{
    class PickUpGunInteractionHandler : InteractionHandler
    {
        World world;

        public PickUpGunInteractionHandler(World world) : base(
            world.GetEntities()
            .With<PlayerComponent>()
            .Build(),
            world.GetEntities()
            .With<PickUpGunComponent>()
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
                    ref var playerTransform = ref interactor.Get<WorldTransformComponent>();
                    ref var gunTransform = ref interactee.Get<WorldTransformComponent>();

                    // Spawn battery
                    interactee.Remove<InteractableComponent>();
                    
                    // Graphical hint disappear
                    ref TextureComponent texture = ref interactee.Get<TextureComponent>();
                    texture.glowing = false;

                    // Add gun to player
                    interactee.Set(new InputComponent(new ShootingInputHandler(world)));
                    interactee.SetAsChildOf(interactor);
                    interactor.Set(new WeaponComponent(interactee));

                    Debug.WriteLine("Gun PickUp Done!");

                    return true;
            }

            return false;
        }
    }
}

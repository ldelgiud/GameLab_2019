﻿using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Hazmat.Input;
using Hazmat.Components;
using Hazmat.Components.InputHandlers;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Interaction.Handlers
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
                case PressEvent _:
                    ref var playerTransform = ref interactor.Get<Transform2DComponent>();
                    ref var gunTransform = ref interactee.Get<Transform2DComponent>();
                    ref var model = ref interactee.Get<ModelComponent>();


                    interactee.Remove<InteractableComponent>();

                    // Disable graphical hint
                    model.DisableToonGlow();

                    // Add gun to player
                    interactee.Set(new InputComponent(new ShootingInputHandler(world)));
                    interactee.SetAsChildOf(interactor);
                    gunTransform.value.Parent = playerTransform.value;
                    gunTransform.value.LocalTranslation = new Vector2(1.414f, 1.414f);


                    interactor.Set(new WeaponComponent(interactee)); 



                    Debug.WriteLine("Gun PickUp Done!");

                    return true;
            }

            return false;
        }
    }
}
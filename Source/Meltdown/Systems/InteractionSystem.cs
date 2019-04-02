﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Interaction;
using Meltdown.Components;
using Meltdown.Input;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class InteractionSystem : AEntitySystem<Time>
    {

        InputManager inputManager;
        IEnumerable<InteractionHandler> interactionHandlers;

        EntitySet players;

        public InteractionSystem(InputManager inputManager, World world, IEnumerable<InteractionHandler> interactionHandlers) : base(
            world.GetEntities()
            .With<InteractableComponent>()
            .With<WorldTransformComponent>()
            .Build()
            )
        {
            this.inputManager = inputManager;
            this.interactionHandlers = interactionHandlers;
            this.players = world.GetEntities().With<PlayerComponent>().With<WorldTransformComponent>().Build();
        }

        protected override void Update(Time state, ReadOnlySpan<Entity> entities)
        {
            foreach (var playerEntity in this.players.GetEntities())
            {
                ref var player = ref playerEntity.Get<PlayerComponent>();
                ref var playerTransform = ref playerEntity.Get<WorldTransformComponent>();

                var inputEvent = this.inputManager.GetEvent(player.Id, Buttons.X);

                if (inputEvent == null && player.Id == 1)
                {
                    inputEvent = this.inputManager.GetEvent(Keys.E);
                }

                if (inputEvent != null)
                {
                    foreach (var entity in entities)
                    {
                        ref var entityTransform = ref entity.Get<WorldTransformComponent>();

                        if (Vector2.Distance(entityTransform.value.position.ToVector2(), playerTransform.value.position.ToVector2()) < Constants.INTERACTION_DISTANCE)
                        {
                            foreach (var handler in this.interactionHandlers)
                            {
                                bool remove = handler.HandleInteractions(inputEvent, playerEntity, entity);
                                if (remove)
                                {
                                    this.inputManager.RemoveEvent(player.Id, Buttons.X);
                                    if (player.Id == 1)
                                    {
                                        this.inputManager.RemoveEvent(Keys.E);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

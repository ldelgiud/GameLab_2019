using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

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

        Effect interactionEffect;

        public InteractionSystem(InputManager inputManager, World world, IEnumerable<InteractionHandler> interactionHandlers, Effect interactionEffect) : base(
            world.GetEntities()
            .With<InteractableComponent>()
            .With<Transform2DComponent>()
            .With<WorldSpaceComponent>()
            .Build()
            )
        {
            this.inputManager = inputManager;
            this.interactionHandlers = interactionHandlers;
            this.players = world.GetEntities()
                .With<PlayerComponent>()
                .With<Transform2DComponent>()
                .With<WorldSpaceComponent>()
                .Build();
            this.interactionEffect = interactionEffect;
            interactionEffect.Parameters["u_blurSize"].SetValue(0.05f);
            interactionEffect.Parameters["u_intensity"].SetValue(2.5f);
        }

        protected override void Update(Time state, ReadOnlySpan<Entity> entities)
        {
            foreach (var playerEntity in this.players.GetEntities())
            {
                ref var player = ref playerEntity.Get<PlayerComponent>();
                ref var playerTransform = ref playerEntity.Get<Transform2DComponent>();

                var inputEvent = this.inputManager.GetEvent(player.Id, Buttons.X);

                if (inputEvent == null && player.Id == 1)
                {
                    inputEvent = this.inputManager.GetEvent(Keys.E);
                }

                foreach (var entity in entities)
                {
                    ref var entityTransform = ref entity.Get<Transform2DComponent>();
                    ref InteractableComponent interactable = ref entity.Get<InteractableComponent>();

                    if (Vector2.Distance(entityTransform.value.Translation, playerTransform.value.Translation) < Constants.INTERACTION_DISTANCE)
                    {
                        // If player wasn't already nearby the interactable object then trigger the glowing (only once until player moves out of distance)
                        if (!interactable.playerNearby && entity.Has<Texture2DComponent>())
                        {
                            ref Texture2DComponent texture = ref entity.Get<Texture2DComponent>();
                            texture.SetTemporaryEffect(interactionEffect, "time");
                            interactable.playerNearby = true;
                            Debug.WriteLine("Interactable");
                        }
                        else if (!interactable.playerNearby && entity.Has<ModelComponent>())
                        {
                            ref ModelComponent model = ref entity.Get<ModelComponent>();
                            model.EnableToonGlow();
                            interactable.playerNearby = true;
                        }

                        if (inputEvent != null)
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
                    else
                    {
                        // If player was nearby and now is not anymore, then make glowing disappear
                        if (interactable.playerNearby && entity.Has<Texture2DComponent>())
                        {
                            ref Texture2DComponent texture = ref entity.Get<Texture2DComponent>();
                            texture.RemoveTemporaryEffect();
                            interactable.playerNearby = false;

                        }
                        else if (interactable.playerNearby && entity.Has<ModelComponent>())
                        {
                            ref ModelComponent model = ref entity.Get<ModelComponent>();
                            model.DisableToonGlow();
                            interactable.playerNearby = false;
                        }

                    }
                }
            }
        }

    }
}

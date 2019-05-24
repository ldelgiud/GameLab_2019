using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Interaction;
using Hazmat.Components;
using Hazmat.Input;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
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
            .With<Transform3DComponent>()
            .With<WorldSpaceComponent>()
            .Build()
            )
        {
            this.inputManager = inputManager;
            this.interactionHandlers = interactionHandlers;
            this.players = world.GetEntities()
                .With<PlayerComponent>()
                .With<Transform3DComponent>()
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
                ref var playerTransform = ref playerEntity.Get<Transform3DComponent>();

                //var inputEvent = this.inputManager.GetEvent(player.Id, Buttons.X);

                //if (inputEvent == null && player.Id == 0)
                //{
                //    inputEvent = this.inputManager.GetEvent(Keys.E);
                //}

                foreach (var entity in entities)
                {
                    ref var entityTransform = ref entity.Get<Transform3DComponent>();
                    ref InteractableComponent interactable = ref entity.Get<InteractableComponent>();

                    var interactionDistance = (float)Constants.INTERACTION_DISTANCE;

                    if (entity.Has<AABBComponent>())
                    {
                        // Quick fix for interactable objects
                        ref var aabb = ref entity.Get<AABBComponent>();
                        interactionDistance += MathF.Max((float)aabb.aabb.Height, (float)aabb.aabb.Width);
                    }

                    if (Vector2.Distance(entityTransform.value.Translation.ToVector2(), playerTransform.value.Translation.ToVector2()) < interactionDistance)
                    {
                        // If player wasn't already nearby the interactable object then trigger the glowing (only once until player moves out of distance)
                        if (!interactable.playerNearby && entity.Has<Texture2DComponent>())
                        {
                            ref Texture2DComponent texture = ref entity.Get<Texture2DComponent>();
                            texture.SetTemporaryEffect(interactionEffect, "time");
                            interactable.playerNearby = true;
                        }
                        else if (!interactable.playerNearby && entity.Has<ModelComponent>())
                        {
                            ref ModelComponent model = ref entity.Get<ModelComponent>();
                            model.EnableToonGlow();
                            interactable.playerNearby = true;
                        }

                        //if (inputEvent != null)
                        //{
                            foreach (var handler in this.interactionHandlers)
                            {
                                bool remove = handler.HandleInteractions(null, playerEntity, entity);
                                if (remove)
                                {
                                    this.inputManager.RemoveEvent(player.Id, Buttons.X);
                                    if (player.Id == 0)
                                    {
                                        this.inputManager.RemoveEvent(Keys.E);
                                    }
                                }
                            }
                        }
                    //}
                    //else
                    //{
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

                    //}
                }
            }
        }

    }
}

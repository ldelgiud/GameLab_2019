﻿using System;
using System.Collections.Generic;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Hazmat.Input;
using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Components.InputHandlers
{
    class PlayerInputHandler : IInputHandler
    {
        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            ref VelocityComponent velComp = ref entity.Get<VelocityComponent>();
            ref StatsComponent player = ref entity.Get<StatsComponent>();
            ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();

            bool gamepadDirection = false;

            velComp.velocity = Vector2.Zero;

            // GamePad
            switch (inputManager.GetEvent(0, ThumbSticks.Left))
            {
                case ValueEvent<Vector2> value:
                    if (value.current.LengthSquared() != 0)
                    {
                        velComp.velocity = player.Speed * value.current;
                        entity.SetModelAnimationPlaybackSpeed(1);
                        gamepadDirection = true;
                        var current = Camera2D.PerspectiveToWorld(value.current);
                        current.Normalize();
                        var rotation = transform.value.Rotation;
                        transform.value.Rotation = new Vector3(rotation.X, rotation.Y, current.ToRotation());
                    }
                    else
                    {
                        velComp.velocity = Vector2.Zero;
                        entity.SetModelAnimationFrame(0);
                        entity.SetModelAnimationPlaybackSpeed(0f);
                    }
                    break;
            }

            switch (inputManager.GetEvent(0, ThumbSticks.Right))
            {
                case ValueEvent<Vector2> value:
                    var current = Camera2D.PerspectiveToWorld(value.current);
                    if (current.LengthSquared() != 0)
                    {
                        if (entity.Has<WeaponComponent>() && entity.Get<WeaponComponent>().weapon.Has<SmallGunComponent>())
                        {
                            entity.Get<WeaponComponent>().weapon.Get<SmallGunComponent>()
                                .Shoot(time.Absolute, entity.Get<WeaponComponent>().weapon.Get<Transform3DComponent>().value, current, new Vector3(1.5f, 0f, 1.0f));
                        }
                        gamepadDirection = true;
                        current.Normalize();
                        var rotation = transform.value.Rotation;
                        transform.value.Rotation = new Vector3(rotation.X, rotation.Y, current.ToRotation());
                    }
                    break;
            }

            if (!GamePad.GetState(0).IsConnected)
            {
                // Mouse Rotation
                MouseState mState = Mouse.GetState();
                Vector2 dir = Camera2D.WorldToPerspective(mState.Position.ToVector2() - Hazmat.Instance.Window.ClientBounds.Center.ToVector2());
                dir.Normalize();
                if (dir.LengthSquared() != 0 && !gamepadDirection)
                {
                    var rotation = transform.value.Rotation;
                    transform.value.Rotation = new Vector3(rotation.X, rotation.Y, -dir.ToRotation());
                }
            }
            

            // KeyBoard
            switch (inputManager.GetEvent(Keys.Left))
            {
                case ReleaseEvent _: break;
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Right))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Up))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.Down))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -player.Speed;
                    break;
            }

            // KeyBoard - WASD
            switch (inputManager.GetEvent(Keys.A))
            {
                case ReleaseEvent _: break;
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = -player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.D))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.X = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.W))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = player.Speed;
                    break;
            }
            switch (inputManager.GetEvent(Keys.S))
            {
                case HoldEvent _:
                case PressEvent _:
                    velComp.velocity.Y = -player.Speed;
                    break;
            }




            if (velComp.velocity != Vector2.Zero)
            {
                velComp.velocity = Camera2D.PerspectiveToWorld(velComp.velocity);
                velComp.velocity.Normalize();
                velComp.velocity *= player.Speed;
            }
            
        }
    }
}

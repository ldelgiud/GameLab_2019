﻿using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Hazmat.State;
using Hazmat.Components;
using Hazmat.Systems;
using Hazmat.ResourceManagers;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.Input;
using Hazmat.Graphics;

using Spine;

namespace Hazmat.States
{
    public class CreditsState : State.State
    {
        GameWindow window;

        InputManager inputManager;

        World world;
        Camera2D screenCamera;

        ISystem<Time> drawSystem;

        public override void Initialize(Time time, Hazmat game)
        {
            this.window = game.Window;

            this.inputManager = new InputManager();
            // Input 
            this.inputManager.Register(Keys.Escape);
            this.inputManager.Register(Buttons.B);

            this.world = new World();
            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            // Resource Managers
            Hazmat.Instance.spineAnimationResourceManager.Manage(this.world);

            this.drawSystem = new SequentialSystem<Time>(
                new AnimationStateUpdateSystem(this.world),
                new SkeletonUpdateSystem(this.world),
                new SpineSkeleton2DDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world)
                );

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D()));
                entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                    new SpineAnimationInfo(
                        @"ui\SPS_Screens",
                        new SkeletonInfo(1920, 1080, skin: "credits"),
                        null
                    )
                ));
            }
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent backEvent = this.inputManager.GetEvent(Keys.Escape) ?? this.inputManager.GetEvent(0, Buttons.B);

            switch (backEvent)
            {
                case ReleaseEvent _:
                    this.stateTransition = new PopStateTransition(null);
                    break;
            }

            return base.Update(time);
        }

        public override void Draw(Time time)
        {
            this.screenCamera.Update(this.window);
            this.drawSystem.Update(time);
        }
    }
}

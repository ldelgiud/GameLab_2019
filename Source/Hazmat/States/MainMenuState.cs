using System;
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
    public class MainMenuState : State.State
    {
        GameWindow window;

        InputManager inputManager;

        World world;
        Camera2D screenCamera;

        SpineAnimationResourceManager spineAnimationResourceManager;

        ISystem<Time> drawSystem;

        public override void Initialize(Hazmat game)
        {
            this.window = game.Window;

            this.inputManager = new InputManager();
            // Input 
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);
            this.inputManager.Register(Buttons.B);

            this.world = new World();
            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            // Resource Managers
            this.spineAnimationResourceManager = new SpineAnimationResourceManager(game.GraphicsDevice);
            this.spineAnimationResourceManager.Manage(this.world);

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
                        new SkeletonInfo(1920, 1080, skin: "main_menu"),
                        null
                    )
                ));
            }
        }

        public override void Resume(object data)
        {
            this.inputManager.Clear();
            this.inputManager.Sleep(10);
            base.Resume(data);
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent inputEvent = this.inputManager.GetEvent(Keys.Enter) ?? this.inputManager.GetEvent(0, Buttons.A);
            IInputEvent exitEvent = this.inputManager.GetEvent(Keys.Escape) ?? this.inputManager.GetEvent(0, Buttons.B);

            switch (inputEvent)
            {
                case ReleaseEvent _:
                    this.stateTransition = new PushStateTransition(new GameState());
                    break;
            }

            switch (exitEvent)
            {
                case ReleaseEvent _:
                    this.stateTransition = new ExitTransition();
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

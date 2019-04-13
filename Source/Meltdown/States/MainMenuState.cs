using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Meltdown.State;
using Meltdown.Components;
using Meltdown.Systems;
using Meltdown.ResourceManagers;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Meltdown.Input;
using Meltdown.Graphics;

using Spine;

namespace Meltdown.States
{
    public class MainMenuState : State.State
    {
        StateTransition transition;

        GameWindow window;

        InputManager inputManager;

        World world;
        Camera2D screenCamera;

        SpineAnimationResourceManager spineAnimationResourceManager;

        ISystem<Time> drawSystem;

        public override void Initialize(Game1 game)
        {
            this.window = game.Window;
            this.transition = new StateTransition();

            this.inputManager = new InputManager();
            // Input 
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);

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
                new SpineSkeletonDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world)
                );

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D()));
                entity.Set(new ManagedResource<SpineAnimationInfo, SpineAnimationAlias>(
                    new SpineAnimationInfo(
                        @"menu\main\screens", 
                        new SkeletonInfo(1920, 1080, skin: "main_menu"),
                        null
                    )
                ));
            }
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent inputEvent = this.inputManager.GetEvent(Keys.Enter) ?? this.inputManager.GetEvent(0, Buttons.A);

            switch (inputEvent)
            {
                case ReleaseEvent _:
                    return new SwapTransition(new GameState());
            }

            return null;
        }

        public override void Draw(Time time)
        {
            this.screenCamera.Update(this.window);
            this.drawSystem.Update(time);
        }
    }
}

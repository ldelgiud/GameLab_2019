using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazmat.State;
using Hazmat.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Hazmat.Input;
using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.ResourceManagers;
using Hazmat.Systems;
using Hazmat.Event;
using Hazmat.Utilities;

namespace Hazmat.States
{
    class CoverState : State.State
    {
        GameWindow window;

        InputManager inputManager;
        Camera2D screenCamera;
        World world;

        SpineAnimationResourceManager spineAnimationResourceManager;

        ISystem<Time> drawSystem;

        public override void Initialize(Hazmat game)
        {
            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);
            this.SetInstance(this.inputManager);

            this.window = game.Window;

            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            // World
            this.world = new World();

            // Resource Managers
            this.spineAnimationResourceManager = new SpineAnimationResourceManager(game.GraphicsDevice);
            this.spineAnimationResourceManager.Manage(this.world);

            // Systems
            this.drawSystem = new SequentialSystem<Time>(
                new AnimationStateUpdateSystem(this.world),
                new SkeletonUpdateSystem(this.world),
                new SpineSkeletonDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world)
                );

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(new SpineAnimationInfo(
                    @"menu\main\screens",
                    new SkeletonInfo(1920, 1080, skin: "press_any_key"),
                    new AnimationStateInfo("screen_splash_idle", true)
                )));
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D()));
            }
            
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent inputEvent = this.inputManager.GetEvent(Keys.Enter) ?? this.inputManager.GetEvent(0, Buttons.A);

            switch (inputEvent)
            {
                case ReleaseEvent _:
                    this.stateTransition = new SwapStateTransition(new MainMenuState());
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

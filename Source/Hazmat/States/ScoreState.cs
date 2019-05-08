using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Hazmat.State;
using Hazmat.Components;
using Hazmat.ResourceManagers;
using Hazmat.Systems;
using Hazmat.Input;
using Hazmat.Graphics;
using Hazmat.Utilities;

namespace Hazmat.States
{
    class ScoreState : State.State
    {
        GameWindow window;
        InputManager inputManager;
        Camera2D screenCamera;

        Score score;
        World world;

        TextResourceManager textResourceManager;
        SpineAnimationResourceManager spineAnimationResourceManager;

        ISystem<Time> drawSystem;

        public ScoreState(Score score)
        {
            this.score = score;
        }

        public override void Initialize(Time time, Hazmat game)
        {
            this.window = game.Window;
            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);
            this.SetInstance(this.inputManager);

            this.world = new World();
            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            // Resource Managers
            Hazmat.Instance.spineAnimationResourceManager.Manage(this.world);
            Hazmat.Instance.textResourceManager.Manage(this.world);

            this.drawSystem = new SequentialSystem<Time>(
                new AnimationStateUpdateSystem(this.world),
                new SkeletonUpdateSystem(this.world),
                new SpineSkeleton2DDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world),
                new ScreenTextSystem(game.GraphicsDevice, this.screenCamera, this.world)
                );

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(new SpineAnimationInfo(
                    @"ui\SPS_Screens",
                    new SkeletonInfo(1920, 1080, skin: "player_stats"),
                    new AnimationStateInfo("press_A_to_continue", true)
                )));
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D()));
            }

            var color = new Color(22, 28, 47);

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 170))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    new DateTime((long)((this.score.TimeEnd - this.score.TimeStart) * 10e6f)).ToString("mm:ss"),
                    @"font\Playtime",
                    color: color
                    )));
            }
            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 105))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.Kills.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 40))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.Batteries.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, -25))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.ArmorUpgrades.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, -90))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.WeaponUpgrades.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent inputEvent = this.inputManager.GetEvent(Keys.Enter) ?? this.inputManager.GetEvent(0, Buttons.A);

            switch (inputEvent)
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

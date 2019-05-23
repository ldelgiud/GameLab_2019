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
using Hazmat.Utilities.Extensions;

namespace Hazmat.States
{
    class ScoreState : State.State
    {
        static float OFFSET_Y = -100;

        GameWindow window;
        InputManager inputManager;
        Camera2D screenCamera;
        EntitySet players;
        PowerPlant plant;
        Score score;
        World world;

        ISystem<Time> drawSystem;

        public ScoreState(Score score, PowerPlant plant, EntitySet players)
        {
            this.score = score;
            this.plant = plant;
            this.players = players;
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
                    new SkeletonInfo(1920, 1080, skin: this.score.Won ? "player_stats_victory" : "player_stats_defeat"),
                    new AnimationStateInfo("press_A_to_continue", true)
                )));
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(position: new Vector2(0, OFFSET_Y))));
            }

            var color = new Color(22, 28, 47);

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 235 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    new DateTime((long)((this.score.TimeEnd - this.score.TimeStart) * 10e6f)).ToString("mm:ss"),
                    @"font\Playtime",
                    color: color
                    )));
            }
            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 170 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.Kills.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 105 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.Batteries.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, 40 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.ArmorUpgrades.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, -25 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    this.score.WeaponUpgrades.ToString(),
                    @"font\Playtime",
                    color: color
                    )));
            }

            {
                Vector2 avgDist = Vector2.Zero;
                foreach(Entity player in this.players.GetEntities())
                {
                    avgDist += player.Get<Transform3DComponent>().value.Translation.ToVector2();
                }
                avgDist /= players.Count;
                avgDist = plant.Position - avgDist;
                double percentage = (avgDist.Length() / Constants.PLANT_PLAYER_DISTANCE);
                percentage = Math.Min(1,percentage);
                //TODO: Pass win lose value
                percentage = Math.Min((1 - percentage) * 100 , 100);
                if (this.score.Won) percentage = 100;
                
                int myVal = (int)percentage;
                var entity = this.world.CreateEntity();
                entity.Set(new ScreenSpaceComponent());
                entity.Set(new Transform2DComponent(new Transform2D(new Vector2(175, -85 + OFFSET_Y))));
                entity.Set(new ManagedResource<TextInfo, TextAlias>(new TextInfo(
                    myVal.ToString() + "%",
                    @"font\Playtime",
                    color: color
                    )));


            }

            this.inputManager.Clear();
            this.inputManager.Sleep(100);

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

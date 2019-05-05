using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.Resource;

using Hazmat.Input;
using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.State;
using Hazmat.States;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Event
{
    class StoryConclusionEvent : Event
    {
        private enum State
        {
            Start,
            Conclusion1,
            Done,
        }

        InputManager inputManager;

        State state = State.Start;
        Entity eventEntity;
        Entity conclusionEntity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;

            this.conclusionEntity = world.CreateEntity();
            this.conclusionEntity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
            this.conclusionEntity.Set(new ScreenSpaceComponent());
            this.conclusionEntity.Set(new NameComponent() { name = "conclusion" });

            this.inputManager = Hazmat.Instance.ActiveState.GetInstance<InputManager>();
        }


        public override void Update(Time time, World world)
        {
            var inputEvent = this.inputManager.GetEvent(0, Buttons.A);

            if (inputEvent == null)
            {
                inputEvent = this.inputManager.GetEvent(Keys.E);
            }


            switch (this.state)
            {
                case State.Start:
                    this.conclusionEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                        new SpineAnimationInfo(@"ui\SPS_Screens",
                        new SkeletonInfo(745, 360, skin: "story_end_01"),
                        new AnimationStateInfo("press_A_to_continue", true)
                        )));

                    this.state = State.Conclusion1;
                    break;
                case State.Conclusion1:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            this.conclusionEntity.Delete();

                            this.inputManager.RemoveEvent(Keys.E);
                            this.state = State.Done;
                            break;
                    }
                    break;
                case State.Done:
                    var score = Hazmat.Instance.ActiveState.GetInstance<Score>();
                    score.Complete(time);
                    Hazmat.Instance.ActiveState.stateTransition = new SwapStateTransition(new ScoreState(score));
                    this.eventEntity.Delete();
                    break;
            }
        }
    }
}

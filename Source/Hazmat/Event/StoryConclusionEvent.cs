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
using Microsoft.Xna.Framework.Audio;
using Hazmat.Music;

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

        SoundManager soundManager
        {
            get
            {
                return Hazmat.Instance.SoundManager;
            }
        }

        SoundEffectInstance playing;
        InputManager inputManager;
        EntitySet players;
        PowerPlant plant;
        State state = State.Start;
        Entity eventEntity;
        Entity conclusionEntity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;
            this.players = world.GetEntities().With<PlayerComponent>().Build();
            this.plant = Hazmat.Instance.ActiveState.GetInstance<PowerPlant>();
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
                    this.playing = this.soundManager.PlaySoundEffectInstance(
                        effect: soundManager.BossResolution09);
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
                            this.soundManager.StopSoundEffectInstance(playing);
                            this.inputManager.RemoveEvent(Keys.E);
                            this.state = State.Done;
                            break;
                    }
                    break;
                case State.Done:
                    var score = Hazmat.Instance.ActiveState.GetInstance<Score>();
                    score.Complete(time);
                    this.soundManager.PlaySoundEffectInstance(soundManager.MatWin);
                    Hazmat.Instance.ActiveState.stateTransition = 
                        new SwapStateTransition(new ScoreState(score, this.plant, this.players));
                    this.eventEntity.Delete();
                    break;
            }
        }
    }
}

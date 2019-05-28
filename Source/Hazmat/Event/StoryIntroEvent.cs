using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Hazmat.Components;
using Hazmat.Components.InputHandlers;
using Hazmat.Input;
using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.Music;
using Microsoft.Xna.Framework.Audio;

namespace Hazmat.Event
{
    class StoryIntroEvent : Event
    {
        SoundManager soundManager
        {
            get
            {
                return Hazmat.Instance.SoundManager;
            }
        }

        private enum State
        {
            Start,
            Intro0,
            Intro1,
            Intro2,
            Waiting,
            Tutorial,
            Done
        }

        InputManager inputManager;
        State state = State.Start;

        EntitySet players;

        Entity eventEntity;
        SoundEffectInstance playing;
        Entity introEntity;
        float timestamp;
        float timeToStop;
        public bool Answered;
        public override void Initialize(World world, Entity entity)
        {
            this.players = world.GetEntities().With<PlayerComponent>().Build();
            this.eventEntity = entity;

            this.introEntity = world.CreateEntity();
            this.introEntity.Set(new ScreenSpaceComponent());
            this.introEntity.Set(new NameComponent() { name = "intro" });
            this.introEntity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260), scale: new Vector2(0.75f))));
            this.Answered = false;
            this.inputManager = Hazmat.Instance.ActiveState.GetInstance<InputManager>();
        }

        public override void Update(Time time, World world)
        {
            var inputEvent = this.inputManager.GetEvent(0, Buttons.A);
            var closeEvent = this.inputManager.GetEvent(0, Buttons.B);

            if (inputEvent == null)
            {
                inputEvent = this.inputManager.GetEvent(Keys.E);
            }

            if (closeEvent == null)
            {
                closeEvent = this.inputManager.GetEvent(Keys.Q);
            }

            switch (this.state)
            {
                case State.Start:
                    Hazmat.Instance.ActiveState.GetInstance<Energy>().Active = false;
                    this.playing = this.soundManager.PlaySoundEffectInstance(effect: soundManager.Ringtone, loop: true);
                    this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                                new SpineAnimationInfo(@"ui\SPS_Screens",
                                new SkeletonInfo(skin: "story_0"),
                                new AnimationStateInfo("press_A_to_pickup_phone", true)
                                )));
                    this.timeToStop = time.Absolute + 15;
                    this.state = State.Intro0;
                    break;
                case State.Intro0:
                    if (time.Absolute >= this.timeToStop || 
                        (closeEvent != null && closeEvent.GetType() == typeof(PressEvent)))
                    {
                        Hazmat.Instance.ActiveState.GetInstance<Energy>().Active = true;
                        foreach (var player in this.players.GetEntities())
                        {
                            player.Set(new InputComponent(new PlayerInputHandler()));
                        }

                        this.state = State.Tutorial;
                        this.Answered = false;
                    }
                    else if ((inputEvent != null && inputEvent.GetType() == typeof(PressEvent)))
                    {
                        this.Answered = true;

                        this.soundManager.StopSoundEffectInstance(this.playing);
                        this.playing = this.soundManager.PlaySoundEffectInstance(effect: soundManager.BossIntro01);
                        this.timeToStop = time.Absolute + 60;

                        ref var skeleton = ref this.introEntity.Get<SpineSkeletonComponent>();
                        ref var animation = ref this.introEntity.Get<SpineAnimationComponent>();

                        this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                            new SpineAnimationInfo(@"ui\SPS_Screens",
                            new SkeletonInfo(skin: "story_01"),
                            new AnimationStateInfo("tip_popup", false)
                            )));

                        animation = introEntity.Get<SpineAnimationComponent>();
                        animation.value.AddAnimation(0, "press_A_to_continue", true, 0);

                        this.state = State.Intro1;

                        this.inputManager.RemoveEvent(Keys.E);
                        this.inputManager.RemoveEvent(0, Buttons.A);
                    }
                    
                    break;
                    
                case State.Intro1:
                    if ((inputEvent != null && inputEvent.GetType() == typeof(PressEvent)) || 
                        time.Absolute >= this.timeToStop)
                    {
                        this.timeToStop = time.Absolute + 60;
                        ref var skeleton = ref this.introEntity.Get<SpineSkeletonComponent>();
                        ref var animation = ref this.introEntity.Get<SpineAnimationComponent>();

                        this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                            new SpineAnimationInfo(@"ui\SPS_Screens",
                            new SkeletonInfo(skin: "story_02"),
                            new AnimationStateInfo("press_A_to_pickup_phone", true)
                            )));

                        this.inputManager.RemoveEvent(Keys.E);
                        this.inputManager.RemoveEvent(0, Buttons.A);
                        this.state = State.Intro2;
                    }
                    break;
                case State.Intro2:
                    if ((inputEvent != null && inputEvent.GetType() == typeof(PressEvent)) ||
                       time.Absolute >= this.timeToStop)
                    {
                        Hazmat.Instance.ActiveState.GetInstance<Energy>().Active = true;
                        foreach (var player in this.players.GetEntities())
                        {
                            player.Set(new InputComponent(new PlayerInputHandler()));
                        }

                        this.inputManager.RemoveEvent(Keys.E);
                        this.inputManager.RemoveEvent(0, Buttons.A);
                        this.introEntity.Remove<SpineSkeletonComponent>();
                        this.introEntity.Remove<SpineAnimationComponent>();
                        this.state = State.Waiting;
                        this.timestamp = time.Absolute + 4f;
                        this.soundManager.StopSoundEffectInstance(playing);
                        this.playing = this.soundManager.PlaySoundEffectInstance(soundManager.MatOk);
                        this.state = State.Waiting;
                    }
                    break;
                case State.Waiting:
                    if (timestamp <= time.Absolute)
                    {
                        this.timeToStop = time.Absolute + 60;
                        ref var transform = ref this.introEntity.Get<Transform2DComponent>();
                        transform.value.LocalTranslation = transform.value.LocalTranslation * new Vector2(1, 0) + new Vector2(0, -330);
                        this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                            new SpineAnimationInfo(@"ui\SPS_Screens",
                            new SkeletonInfo(596, 288, skin: "tip_after_first_steps"),
                            new AnimationStateInfo("tip_popup", false)
                        )));

                        var animation = introEntity.Get<SpineAnimationComponent>();
                        animation.value.AddAnimation(0, "press_A_to_continue", true, 0);

                        this.state = State.Tutorial;
                        this.soundManager.StopSoundEffectInstance(playing);
                        this.playing = this.soundManager.PlaySoundEffectInstance(soundManager.BossEnergy02);
                    }
                    break;
                case State.Tutorial:
                    if ((inputEvent != null && inputEvent.GetType() == typeof(PressEvent)) ||
                       time.Absolute >= this.timeToStop || 
                       !this.Answered)
                    { 
                        this.inputManager.RemoveEvent(Keys.E);
                        this.inputManager.RemoveEvent(0, Buttons.A);
                        this.inputManager.RemoveEvent(0, Buttons.B);
                        this.inputManager.RemoveEvent(Keys.Q);
                        this.introEntity.Delete();
                        this.state = State.Done;
                    }
                break;
                case State.Done:
                    this.eventEntity.Delete();
                    this.soundManager.StopSoundEffectInstance(playing);
                    break;
            }
        }
    }
}

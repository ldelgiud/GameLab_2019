using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Hazmat.Components;
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
            Intro1,
            Intro2,
            Waiting,
            Tutorial,
            Done
        }

        InputManager inputManager;
        State state = State.Start;

        Entity eventEntity;
        SoundEffectInstance playing;
        Entity introEntity;
        float timestamp;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;

            this.introEntity = world.CreateEntity();
            this.introEntity.Set(new ScreenSpaceComponent());
            this.introEntity.Set(new NameComponent() { name = "intro" });
            this.introEntity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260), scale: new Vector2(0.75f))));

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
                        effect: soundManager.BossIntro01);
                    this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                        new SpineAnimationInfo(@"ui\SPS_Screens",
                        new SkeletonInfo(skin: "story_01"),
                        new AnimationStateInfo("press_A_to_continue", true)
                        )));
                    this.state = State.Intro1;
                    break;
                case State.Intro1:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            
                            ref var skeleton = ref this.introEntity.Get<SpineSkeletonComponent>();
                            ref var animation = ref this.introEntity.Get<SpineAnimationComponent>();

                            skeleton.info.skin = "story_02";
                            skeleton.value.SetSkin("story_02");

                            animation.value.SetAnimation(0, "press_A_to_continue", true);

                            this.inputManager.RemoveEvent(Keys.E);
                            this.inputManager.RemoveEvent(0, Buttons.A);
                            this.state = State.Intro2;
                            break;
                    }
                    break;
                case State.Intro2:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            this.inputManager.RemoveEvent(Keys.E);
                            this.inputManager.RemoveEvent(0, Buttons.A);
                            this.introEntity.Remove<SpineSkeletonComponent>();
                            this.introEntity.Remove<SpineAnimationComponent>();
                            this.state = State.Waiting;
                            this.timestamp = time.Absolute + 5;
                            this.soundManager.StopSoundEffectInstance(playing);
                            this.playing = this.soundManager.PlaySoundEffectInstance(soundManager.MatOk);
                            break;
                    }
                    break;
                case State.Waiting:
                    if (timestamp <= time.Absolute)
                    {
                        ref var transform = ref this.introEntity.Get<Transform2DComponent>();
                        transform.value.LocalTranslation = transform.value.LocalTranslation * new Vector2(1, 0) + new Vector2(0, -350);
                        this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                            new SpineAnimationInfo(@"ui\SPS_Screens",
                            new SkeletonInfo(596, 288, skin: "tip_after_first_steps"),
                            new AnimationStateInfo("press_A_to_continue", true)
                        )));
                        this.state = State.Tutorial;
                        this.soundManager.StopSoundEffectInstance(playing);
                        this.playing = this.soundManager.PlaySoundEffectInstance(soundManager.BossEnergy02);
                    }
                    break;
                case State.Tutorial:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            
                            this.inputManager.RemoveEvent(Keys.E);
                            this.inputManager.RemoveEvent(0, Buttons.A);
                            this.introEntity.Delete();
                            this.state = State.Done;
                            break;
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

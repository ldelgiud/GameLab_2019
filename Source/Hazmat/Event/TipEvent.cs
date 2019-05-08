﻿using System;

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
using Hazmat.Music;
using Microsoft.Xna.Framework.Audio;

namespace Hazmat.Event
{
    class TipEvent : Event
    {
        private enum State
        {
            Start,
            Tip,
            Done,
        }

        String name;

        SoundManager soundManager
        {
            get
            {
                return Hazmat.Instance.SoundManager;
            }
        }

        InputManager inputManager;

        State state = State.Start;
        Entity eventEntity;
        Entity tipEntity;
        SoundEffect sound;
        SoundEffectInstance playing;

        public TipEvent(String name, SoundEffect sound)
        {
            this.name = name;
            this.sound = sound;
        }

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;

            this.tipEntity = world.CreateEntity();
            this.tipEntity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, -350), scale: new Vector2(0.75f))));
            this.tipEntity.Set(new ScreenSpaceComponent());
            this.tipEntity.Set(new NameComponent() { name = "tip" });

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
                    this.playing = soundManager.PlaySoundEffectInstance(sound);
                    this.tipEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                        new SpineAnimationInfo(@"ui\SPS_Screens",
                        new SkeletonInfo(596, 288, skin: this.name),
                        new AnimationStateInfo("press_A_to_continue", true)
                        )));

                    this.state = State.Tip;
                    break;
                case State.Tip:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            this.tipEntity.Delete();

                            this.inputManager.RemoveEvent(Keys.E);
                            this.inputManager.RemoveEvent(0, Buttons.A);
                            this.state = State.Done;
                            break;
                    }
                    break;
                case State.Done:
                    soundManager.StopSoundEffectInstance(playing);

                    this.eventEntity.Delete();
                    break;
            }
        }
    }
}
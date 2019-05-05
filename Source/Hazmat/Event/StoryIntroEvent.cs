﻿using System;
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

namespace Hazmat.Event
{
    class StoryIntroEvent : Event
    {
        private enum State
        {
            Start,
            Intro1,
            Intro2,
            Done
        }

        InputManager inputManager;
        State state = State.Start;

        Entity eventEntity;

        Entity introEntity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;

            this.introEntity = world.CreateEntity();
            this.introEntity.Set(new ScreenSpaceComponent());
            this.introEntity.Set(new NameComponent() { name = "intro" });
            this.introEntity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));

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
                    this.introEntity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                        new SpineAnimationInfo(@"ui\SPS_Screens",
                        new SkeletonInfo(745, 360, skin: "story_01"),
                        new AnimationStateInfo("press_A_to_continue", true)
                        )));
                    this.state = State.Intro1;
                    break;
                case State.Intro1:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            //this.intro1Entity.Delete();

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
                            this.introEntity.Delete();
                            this.state = State.Done;
                            break;
                    }
                    break;
                case State.Done:
                    this.eventEntity.Delete();
                    break;
            }
        }
    }
}

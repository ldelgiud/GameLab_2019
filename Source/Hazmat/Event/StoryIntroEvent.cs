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
        Entity intro1Entity;
        Entity intro2Entity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;
            this.inputManager = Hazmat.Instance.ActiveState.GetInstance<InputManager>();
        }

        public override void Update(World world)
        {
            var inputEvent = this.inputManager.GetEvent(0, Buttons.A);

            if (inputEvent == null)
            {
                inputEvent = this.inputManager.GetEvent(Keys.E);
            }


            switch (this.state)
            {
                case State.Start:
                    this.intro1Entity = world.CreateEntity();
                    this.intro1Entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_Story_01", 745, 360)));
                    this.intro1Entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
                    this.intro1Entity.Set(new ScreenSpaceComponent());
                    this.intro1Entity.Set(new NameComponent() { name = "intro1" });
                    this.state = State.Intro1;
                    break;
                case State.Intro1:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            this.intro1Entity.Delete();

                            this.intro2Entity = world.CreateEntity();
                            this.intro2Entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_Story_02", 745, 360)));
                            this.intro2Entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
                            this.intro2Entity.Set(new ScreenSpaceComponent());
                            this.intro2Entity.Set(new NameComponent() { name = "intro2" });

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
                            this.intro2Entity.Delete();
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

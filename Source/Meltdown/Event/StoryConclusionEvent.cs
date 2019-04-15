using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.Resource;

using Meltdown.Input;
using Meltdown.Components;
using Meltdown.Graphics;
using Meltdown.State;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Event
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
        Entity conclusion1Entity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;
            this.inputManager = Game1.Instance.ActiveState.GetInstance<InputManager>();
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
                    this.conclusion1Entity = world.CreateEntity();
                    this.conclusion1Entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_StoryEnd_01", 745, 360)));
                    this.conclusion1Entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
                    this.conclusion1Entity.Set(new ScreenSpaceComponent());
                    this.conclusion1Entity.Set(new NameComponent() { name = "conclusion1" });
                    this.state = State.Conclusion1;
                    break;
                case State.Conclusion1:
                    switch (inputEvent)
                    {
                        case PressEvent _:
                            this.conclusion1Entity.Delete();

                            this.inputManager.RemoveEvent(Keys.E);
                            this.state = State.Done;
                            break;
                    }
                    break;
                case State.Done:
                    Game1.Instance.ActiveState.stateTransition = new PopStateTransition(null);
                    this.eventEntity.Delete();
                    break;
            }
        }
    }
}

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Meltdown.Components;
using Meltdown.Input;
using Meltdown.Graphics;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Event
{
    class StoryIntroEvent : Event
    {
        InputManager inputManager;
        int state = 0;

        Entity eventEntity;
        Entity intro1Entity;
        Entity intro2Entity;

        public override void Initialize(World world, Entity entity)
        {
            this.eventEntity = entity;
            this.inputManager = Game1.Instance.ActiveState.GetInstance<InputManager>();
        }

        public override void Update(World world)
        {


            switch (this.state)
            {
                case 0:
                    this.intro1Entity = world.CreateEntity();
                    this.intro1Entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_Story_01", 745, 360)));
                    this.intro1Entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
                    this.intro1Entity.Set(new ScreenSpaceComponent());
                    this.intro1Entity.Set(new NameComponent() { name = "intro1" });
                    this.state = 1;
                    break;
                case 1:
                    switch (this.inputManager.GetEvent(Keys.E))
                    {
                        case PressEvent _:
                            this.intro1Entity.Delete();

                            this.intro2Entity = world.CreateEntity();
                            this.intro2Entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_UI_HUD_Story_02", 745, 360)));
                            this.intro2Entity.Set(new Transform2DComponent(new Transform2D(new Vector2(0, 260))));
                            this.intro2Entity.Set(new ScreenSpaceComponent());
                            this.intro2Entity.Set(new NameComponent() { name = "intro2" });

                            this.inputManager.RemoveEvent(Keys.E);
                            this.state = 2;
                            break;
                    }
                    break;
                case 2:
                    switch (this.inputManager.GetEvent(Keys.E))
                    {
                        case PressEvent _:
                            this.inputManager.RemoveEvent(Keys.E);
                            this.intro2Entity.Delete();
                            this.eventEntity.Delete();
                            break;
                    }
                    break;

            }
        }
    }
}

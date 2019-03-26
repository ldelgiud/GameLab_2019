using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Meltdown.Components;
using Meltdown.Input;
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


            switch(this.state)
            {
                case 0:
                    this.intro1Entity = world.CreateEntity();
                    this.intro1Entity.Set(new ManagedResource<string, Texture2D>(@"gui\story\intro1"));
                    this.intro1Entity.Set(new ScreenTransformComponent(null, Matrix.Transpose(Matrix.CreateTranslation(960, 360, 0)), Matrix.Identity, Matrix.Identity));
                    this.intro1Entity.Set(new BoundingRectangleComponent(900, 320));
                    this.state = 1;
                    break;
                case 1:
                    switch(this.inputManager.GetEvent(Keys.E))
                    {
                        case PressEvent _:
                        case ReleaseEvent _:
                            this.intro1Entity.Delete();

                            this.intro2Entity = world.CreateEntity();
                            this.intro2Entity.Set(new ManagedResource<string, Texture2D>(@"gui\story\intro2"));
                            this.intro2Entity.Set(new ScreenTransformComponent(null, Matrix.Transpose(Matrix.CreateTranslation(960, 360, 0)), Matrix.Identity, Matrix.Identity));
                            this.intro2Entity.Set(new BoundingRectangleComponent(900, 320));

                            this.inputManager.RemoveEvent(Keys.E);
                            this.state = 2;
                            break;
                    }
                    break;
                case 2:
                    switch(this.inputManager.GetEvent(Keys.E))
                    {
                        case PressEvent _:
                        case ReleaseEvent _:
                            this.intro2Entity.Delete();
                            this.eventEntity.Delete();
                            break;
                    }
                    break;
                
            }
        }
    }
}

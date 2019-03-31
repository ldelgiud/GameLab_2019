using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Meltdown.State;
using Meltdown.Components;
using Meltdown.Systems;
using Meltdown.ResourceManagers;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Meltdown.Input;
using Meltdown.Graphics;

namespace Meltdown.States
{
    public class MainMenuState : State.State
    {
        StateTransition transition;

        InputManager inputManager;

        World world;
        Camera screenCamera;

        TextureResourceManager textureResourceManager;

        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        public override void Initialize(Game1 game)
        {
            this.transition = new StateTransition();

            this.inputManager = new InputManager();
            // Input 
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);

            var window = game.Window.ClientBounds;

            this.world = new World();
            this.screenCamera = new Camera(
                game.Window,
                new Transform(new Vector3(0, 0, -1)),
                Matrix.CreateOrthographic(1920, 1080, 0, 2)
                );

            this.textureResourceManager = new TextureResourceManager(game.Content);
            this.textureResourceManager.Manage(this.world);

            this.updateSystem = new SequentialSystem<Time>(
                new MenuInputSystem(this.world, this.inputManager, this.transition),
                new MenuPulseSystem(this.world)
                );
            this.drawSystem = new SequentialSystem<Time>(
                new ScreenTextureSystem(game.GraphicsDevice, this.screenCamera, this.world)
                );

            // Main menu
            {
                // Logo
                var logoTransform = new Transform(new Vector3(0, 180, 0));

                var logoEntity = this.world.CreateEntity();
                logoEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\logo"));
                logoEntity.Set(new ScreenTransformComponent(logoTransform));
                logoEntity.Set(new BoundingRectangleComponent(959, 227));

                // Start
                {
                    var startEntity = this.world.CreateEntity();
                    startEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\start"));
                    startEntity.Set(new ScreenTransformComponent(new Transform(new Vector3(-360, -360, 0), Vector3.Zero, Vector3.One, logoTransform)));
                    startEntity.Set(new BoundingRectangleComponent(290, 46));
                    startEntity.Set(new MenuPulseComponent(true, 0.1f));
                }

                // Credits
                {
                    var creditsEntity = this.world.CreateEntity();
                    creditsEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\credits"));
                    creditsEntity.Set(new ScreenTransformComponent(new Transform(new Vector3(0, -360, 0), Vector3.Zero, Vector3.One, logoTransform)));
                    creditsEntity.Set(new BoundingRectangleComponent(191, 46));
                }

                // Quit
                {
                    var quitEntity = this.world.CreateEntity();
                    quitEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\quit"));
                    quitEntity.Set(new ScreenTransformComponent(new Transform(new Vector3(360, -360, 0), Vector3.Zero, Vector3.One, logoTransform)));
                    quitEntity.Set(new BoundingRectangleComponent(111, 46));
                }

            }
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);
            this.updateSystem.Update(time);
            return this.transition.Transition;
        }

        public override void Draw(Time time)
        {
            this.drawSystem.Update(time);
        }
    }
}

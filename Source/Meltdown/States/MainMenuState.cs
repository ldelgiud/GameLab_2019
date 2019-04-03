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

        GameWindow window;

        InputManager inputManager;

        World world;
        Camera2D screenCamera;

        TextureResourceManager textureResourceManager;

        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        public override void Initialize(Game1 game)
        {
            this.window = game.Window;
            this.transition = new StateTransition();

            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.Enter);

            var window = game.Window.ClientBounds;

            this.world = new World();
            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
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
                var logoTransform = new Transform2D(new Vector2(0, 180));

                var logoEntity = this.world.CreateEntity();
                logoEntity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"menu\main\logo")));
                logoEntity.Set(new Transform2DComponent() { value = logoTransform });
                logoEntity.Set(new ScreenSpaceComponent());

                // Start
                {
                    var startEntity = this.world.CreateEntity();
                    startEntity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"menu\main\start")));
                    startEntity.Set(new Transform2DComponent() { value = new Transform2D(new Vector2(-360, -360), 0, Vector2.One, logoTransform) });
                    startEntity.Set(new MenuPulseComponent(true, 0.1f));
                    startEntity.Set(new ScreenSpaceComponent());
                }

                // Credits
                {
                    var creditsEntity = this.world.CreateEntity();
                    creditsEntity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"menu\main\credits")));
                    creditsEntity.Set(new Transform2DComponent() { value = new Transform2D(new Vector2(0, -360), 0, Vector2.One, logoTransform) });
                    creditsEntity.Set(new ScreenSpaceComponent());
                }

                // Quit
                {
                    var quitEntity = this.world.CreateEntity();
                    quitEntity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"menu\main\quit")));
                    quitEntity.Set(new Transform2DComponent() { value = new Transform2D(new Vector2(360, -360), 0, Vector2.One, logoTransform) });
                    quitEntity.Set(new ScreenSpaceComponent());
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
            this.screenCamera.Update(this.window);
            this.drawSystem.Update(time);
        }
    }
}

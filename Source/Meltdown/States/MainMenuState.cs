using System.Diagnostics;

using Microsoft.Xna.Framework;
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

namespace Meltdown.States
{
    public class MainMenuState : State.State
    {
        World world;
        Camera camera;
        Screen screen;

        TextureResourceManager textureResourceManager;

        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        public override void Initialize(Game1 game)
        {
            this.world = new World();
            this.camera = new Camera(game.GraphicsDevice, 100, 100, 0, 0);
            this.screen = new Screen(game.Window, 1920, 1080);

            this.textureResourceManager = new TextureResourceManager(game.Content);
            this.textureResourceManager.Manage(this.world);

            this.updateSystem = new SequentialSystem<Time>(
                new MenuPulseSystem(this.world)
                );
            this.drawSystem = new SequentialSystem<Time>(
                new ScreenTextureSystem(game.GraphicsDevice, this.world, this.screen)
                );

            // Main menu
            {
                // Logo
                var logoTransform = new ScreenTransformComponent(null, Matrix.Transpose(Matrix.CreateTranslation(960, 360, 0)), Matrix.Identity, Matrix.Identity);

                var logoEntity = this.world.CreateEntity();
                logoEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\logo"));
                logoEntity.Set(logoTransform);
                logoEntity.Set(new BoundingRectangleComponent(959, 227));

                // Start
                {
                    var startEntity = this.world.CreateEntity();
                    startEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\start"));
                    startEntity.Set(new ScreenTransformComponent(logoTransform, Matrix.Transpose(Matrix.CreateTranslation(-360, 360, 0)), Matrix.Identity, Matrix.CreateScale(0.75f)));
                    startEntity.Set(new BoundingRectangleComponent(290, 46));
                    startEntity.Set(new MenuPulseComponent(true, 0.1f));
                }

                // Credits
                {
                    var creditsEntity = this.world.CreateEntity();
                    creditsEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\credits"));
                    creditsEntity.Set(new ScreenTransformComponent(logoTransform, Matrix.Transpose(Matrix.CreateTranslation(0, 360, 0)), Matrix.Identity, Matrix.Identity));
                    creditsEntity.Set(new BoundingRectangleComponent(191, 46));
                }

                // Quit
                {
                    var quitEntity = this.world.CreateEntity();
                    quitEntity.Set(new ManagedResource<string, Texture2D>(@"menu\main\quit"));
                    quitEntity.Set(new ScreenTransformComponent(logoTransform, Matrix.Transpose(Matrix.CreateTranslation(360, 360, 0)), Matrix.Identity, Matrix.Identity));
                    quitEntity.Set(new BoundingRectangleComponent(111, 46));
                }

            }


        }

        public override IStateTransition Update(Time time)
        {
            this.updateSystem.Update(time);
            return null;
        }

        public override void Draw(Time time)
        {
            this.drawSystem.Update(time);
        }
    }
}

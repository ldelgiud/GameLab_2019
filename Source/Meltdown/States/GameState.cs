using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using tainicom.Aether.Physics2D.Collision;

using Meltdown.State;
using Meltdown.Systems;
using Meltdown.Collision;
using Meltdown.Collision.Handlers;
using Meltdown.Components;
using Meltdown.ResourceManagers;
using Meltdown.Event;
using Meltdown.Utilities;
using Meltdown.Input;
using Meltdown.Graphics;
using Meltdown.Systems.Debugging;
using Meltdown.Pathfinding;

namespace Meltdown.States
{
    class GameState : State.State
    {
        GameWindow window;
        InputManager inputManager;
        Camera2D worldCamera;
        Camera2D screenCamera;
        World world;
        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        TextureResourceManager textureResourceManager;
        ModelResourceManager modelResourceManager;

        public override void Initialize(Game1 game)
        {
            this.inputManager = new InputManager();
            this.SetUpInputManager();
            this.SetInstance(this.inputManager);

            this.window = game.Window;

            Energy energy = new Energy();
            PowerPlant powerPlant = new PowerPlant();

            this.SetInstance(new QuadTree<Entity>(
                new AABB()
                {
                    LowerBound = Constants.BOTTOM_LEFT_CORNER,// new Vector2(-10000, -10000),
                    UpperBound = Constants.TOP_RIGHT_CORNER// new Vector2(10000, 10000)
                },
                10, 7));

            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            this.worldCamera = new Camera2D(
                new Transform2D(),
                100,
                60
                );
            
            this.world = new World();
            this.SetInstance(this.world);

            // Resource Managers
            this.textureResourceManager = new TextureResourceManager(game.Content);
            this.textureResourceManager.Manage(this.world);

            this.modelResourceManager = new ModelResourceManager(game.Content);
            this.modelResourceManager.Manage(this.world);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                //new DebugCollisionHandler(this.world),
                new DamageHealthCollisionHandler(this.world),
                new EnergyPickupCollisionHandler(this.world, energy),
                new EventTriggerCollisionHandler(this.world),
                new PlayerDamageCollisionHandler(this.world, energy)
            });

            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, this.GetInstance<QuadTree<Entity>>(), collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world, this.inputManager);
            EventSystem eventSystem = new EventSystem(this.world);
            AISystem aISystem = new AISystem(this.world);
            PowerplantSystem powerplantSystem =
                new PowerplantSystem(this.world, energy, powerPlant);

            ShootingSystem shootingSystem = new ShootingSystem(this.world, this.inputManager);
            CameraSystem cameraSystem = new CameraSystem(this.worldCamera, this.world);
            TTLSystem TTLSystem = new TTLSystem(world);
            PathFinderSystem pathFinderSystem = new PathFinderSystem();
            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                shootingSystem,
                collisionSystem,
                eventSystem,
                aISystem,
                powerplantSystem,
                cameraSystem,
                TTLSystem,
                pathFinderSystem
                );

            //TERRAIN GENERATION
            //ProcGen.BuildWalls();
            ProcGen.BuildBackground();
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant);
            ProcGen.BuildStreet(powerPlant);
            SpawnHelper.SpawnBasicWall(new Vector2(30,0),10,10);
            Grid grid = new Grid();
            this.SetInstance(new PathRequestManager(new PathFinder(grid)));

            //DRAWING SYSTEMS
            EnergyDrawSystem energyDrawSystem =
                new EnergyDrawSystem(
                    energy : energy,
                    texture : game.Content.Load<Texture2D>("placeholders/EnergyBar PLACEHOLDER"),
                    graphicsDevice : game.GraphicsDevice,
                    font : game.Content.Load<SpriteFont>("gui/EnergyFont")
                    );
            ModelDrawSystem modelDrawSystem = new ModelDrawSystem(this.worldCamera, this.world);
            AABBDebugDrawSystem aabbDebugDrawSystem = new AABBDebugDrawSystem(world, game.GraphicsDevice, this.worldCamera, game.Content.Load<Texture2D>("boxColliders"));


            GraphDrawSystem gridDrawSystem = new GraphDrawSystem(
                grid : grid, 
                graphicsDevice : game.GraphicsDevice,
                camera : this.worldCamera,
                circle : game.Content.Load<Texture2D>("graph/circle-16")
                );

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.worldCamera, this.world),
                new ScreenTextureSystem(game.GraphicsDevice, this.screenCamera, this.world),
                modelDrawSystem,
                //gridDrawSystem,      
                energyDrawSystem,
                aabbDebugDrawSystem
                );
            

            //SPAWNING 
            //ENEMY SPAWNING
            ProcGen.SpawnHotspots();
            // Create player
            SpawnHelper.SpawnPlayer(1);
            // Create energy pickup
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, new Vector2(-20, 20));
            //Spawn one Drone
            SpawnHelper.SpawnDrone(new Vector2(50, 0));
            // Event trigger
            //SpawnHelper.SpawnEvent(new Vector2(0, -20));
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);
            this.updateSystem.Update(time);
            return null;
        }

        public override void Draw(Time time)
        {
            this.screenCamera.Update(this.window);
            this.worldCamera.Update(this.window);
            this.drawSystem.Update(time);
        }

        // Helper Methods
        private void SetUpInputManager()
        {
            // KEYBOARD
            // Player - Keyboard
            this.inputManager.Register(Keys.Up);
            this.inputManager.Register(Keys.Down);
            this.inputManager.Register(Keys.Left);
            this.inputManager.Register(Keys.Right);

            this.inputManager.Register(Keys.D);
            this.inputManager.Register(Keys.A);
            this.inputManager.Register(Keys.S);
            this.inputManager.Register(Keys.W);

            // Shooting - Keyboard 
            this.inputManager.Register(Keys.F);

            // Event - Keyboard
            this.inputManager.Register(Keys.E);

            // GAMEPAD
            // Player - Gamepad 
            this.inputManager.Register(Buttons.LeftThumbstickDown);
            this.inputManager.Register(Buttons.LeftThumbstickUp);
            this.inputManager.Register(Buttons.LeftThumbstickLeft);
            this.inputManager.Register(Buttons.LeftThumbstickRight);

            // Shooting - Gamepad
            this.inputManager.Register(Buttons.RightTrigger);
            this.inputManager.Register(ThumbSticks.Right);

            // Event - Keyboard
            this.inputManager.Register(Buttons.B);
        }

    }
}

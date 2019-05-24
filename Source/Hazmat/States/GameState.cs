using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.State;
using Hazmat.Systems;
using Hazmat.Collision;
using Hazmat.Collision.Handlers;
using Hazmat.Components;
using Hazmat.ResourceManagers;
using Hazmat.Event;
using Hazmat.Utilities;
using Hazmat.Input;
using Hazmat.Interaction;
using Hazmat.Interaction.Handlers;
using Hazmat.Graphics;
using Hazmat.Systems.Debugging;
using Hazmat.Pathfinding;
using Hazmat.PostProcessor;

namespace Hazmat.States
{
    class GameState : State.State
    {
        GameWindow window;
        InputManager inputManager;
        Camera3D worldCamera;
        Camera2D screenCamera;
        World world;
        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        RenderCapture renderCapture;
        PostProcessing postprocessor;

        TileMap tileMap;

        public override void Initialize(Time time, Hazmat game)
        {
            this.inputManager = new InputManager();
            this.SetUpInputManager();
            this.SetInstance(this.inputManager);
            this.window = game.Window;

            ConfigurePostProcessor(game);

            Score score = new Score(time);
            this.SetInstance(score);

            Energy energy = new Energy();
            this.SetInstance(energy);
            PowerPlant powerPlant = new PowerPlant();
            this.SetInstance(powerPlant);
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

            this.worldCamera = new Camera3D(
                new Transform3D(),
                40,
                71,
                40
                );
            
            this.world = new World();
            this.SetInstance(this.world);

            // Music
            Hazmat.Instance.SoundManager.PlayBackgroundMusic(Hazmat.Instance.SoundManager.InGameSong, loop: true);
            // Resource Managers
            Hazmat.Instance.textureResourceManager.Manage(this.world);
            Hazmat.Instance.modelResourceManager.Manage(this.world);
            Hazmat.Instance.spineAnimationResourceManager.Manage(this.world);
            Hazmat.Instance.atlasTextureResourceManager.Manage(this.world);

            // Miscellaneous
            this.tileMap = new TileMap(game.GraphicsDevice, @"items\SPS_StaticSprites");
            this.SetInstance(tileMap);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                //new DebugCollisionHandler(this.world),
                new DamageHealthCollisionHandler(this.world, score),
                new EnergyPickupCollisionHandler(this.world, energy, score),
                new EventTriggerCollisionHandler(this.world),
                new PlayerDamageCollisionHandler(this.world, energy),
                new PowerUpPickUpCollisionHandler(this.world, score),
                new DamageSolidCollisionHandler(this.world),
            });

            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, this.GetInstance<QuadTree<Entity>>(), collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world, this.inputManager);
            EventSystem eventSystem = new EventSystem(this.world);
            AISystem aISystem = new AISystem(this.world, this.worldCamera);
            PowerplantSystem powerplantSystem =
                new PowerplantSystem(this.world, energy, powerPlant);

            ShootingSystem shootingSystem = new ShootingSystem(this.world, this.inputManager);
            CameraSystem cameraSystem = new CameraSystem(this.worldCamera, this.world);
            TTLSystem TTLSystem = new TTLSystem(world);
            PathFinderSystem pathFinderSystem = new PathFinderSystem();
            InteractionSystem interactionSystem = new InteractionSystem(
                this.inputManager,
                this.world,
                new InteractionHandler[] {
                    new LootableInteractionHandler(this.world),
                    new PickUpGunInteractionHandler(this.world),
                    new PowerPlantInteractionHandler(this.world),
                    },
                    Hazmat.Instance.Content.Load<Effect>(@"shaders/bright")
                );
            PowerUpSystem powerUpSystem = new PowerUpSystem(this.world);
            DamageEffectSystem damageEffectSystem = new DamageEffectSystem(this.world);
            LowEnergyEffectSystem lowEnergyEffectSystem = new LowEnergyEffectSystem(this.world, energy, this.postprocessor);

            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                new AABBTetherSystem(this.world),
                eventSystem,
                interactionSystem,
                collisionSystem,
                damageEffectSystem,
                lowEnergyEffectSystem,
                aISystem,
                powerUpSystem,
                powerplantSystem,
                cameraSystem,
                TTLSystem,
                pathFinderSystem,
                new TutorialSystem(world, powerPlant),
                new EnergyEventSystem(energy, score, powerPlant, world)
                );

            //TERRAIN GENERATION
            SpawnMap spawnMap = new SpawnMap();
            this.SetInstance(spawnMap);
            Street street = new Street();
            this.SetInstance(street);

            ProcGen.BuildBackground();
            ProcGen.BuildPowerPlant(powerPlant);
            SpawnHelper.SpawnPlayerHouse();
            SpawnHelper.SpawnLootStation(new Vector2(-20,-15), 0);

            ProcGen.BuildStreet(powerPlant);
            ProcGen.BuildExtras();
            ProcGen.SetSpawnRates();
            Grid grid = new Grid();

            this.SetInstance(new PathRequestManager(new PathFinder(grid)));
            
            //DRAWING SYSTEMS
            EnergyDrawSystem energyDrawSystem =
                new EnergyDrawSystem(
                    energy,
                    this.world
                    );
            AABBDebugDrawSystem aabbDebugDrawSystem = new AABBDebugDrawSystem(world, game.GraphicsDevice, this.worldCamera, game.Content.Load<Texture2D>(@"debugging\bounding_box"));
            /*
            GraphDrawSystem gridDrawSystem = new GraphDrawSystem(
                grid : grid, 
                graphicsDevice : game.GraphicsDevice,
                camera : this.worldCamera,
                circle : game.Content.Load<Texture2D>("graph/circle-16")
                );
            */

            this.drawSystem = new SequentialSystem<Time>(
                // State update systems
                new ModelAnimationUpdateSystem(this.world),
                new AnimationStateUpdateSystem(this.world),
                new SkeletonUpdateSystem(this.world),

                // World draw systems
                new TileMapDrawSystem(game.GraphicsDevice, this.worldCamera, this.tileMap),
                new SpatialDrawSystem(game.GraphicsDevice,
                    this.world,
                    this.worldCamera,
                    game.Content.Load<Texture2D>(@"ui\HealthBar_W"),
                    game.Content.Load<Texture2D>(@"ui\HealthBar_G")
                    ),
                //gridDrawSystem,
                aabbDebugDrawSystem,


                // Screen draw systems
                energyDrawSystem,
                new SpineSkeleton2DDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world),
                new ScreenTextureSystem(game.GraphicsDevice, this.screenCamera, this.world)
                );


            //-----------------------SPAWNING-----------------------
            SpawnHelper.SpawnPowerUp(new Vector2(10,10));
            SpawnHelper.SpawnPowerUp(new Vector2(10,25));
            SpawnHelper.SpawnPowerUp(new Vector2(10,40));

            //ENEMY SPAWNING
            SpawnHelper.SpawnEnemyCamps();
            SpawnHelper.SpawnEnemyCamp(new Vector2(100, 100));
            // Create player
            SpawnHelper.SpawnPlayer(0, Vector2.Zero);
            // Story Intro Event
            StoryIntroEvent intro = SpawnHelper.SpawnEvent();
            this.SetInstance(intro);
            //-----------------------END SPAWNING-----------------------
        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);
            this.updateSystem.Update(time);

            return base.Update(time);
        }

        public override void Draw(Time time)
        {
            this.screenCamera.Update(this.window);
            this.drawSystem.Update(time);
        }

        // Helper Methods
        private void ConfigurePostProcessor(Hazmat game)
        {
            // PostProcessing
            renderCapture = new RenderCapture(Hazmat.Instance.GraphicsDevice);
            Effect contrastEffect = game.Content.Load<Effect>(@"shaders/post");
            // Vignette
            contrastEffect.Parameters["radiusX"].SetValue(0.5f); //0.5
            contrastEffect.Parameters["radiusY"].SetValue(0.37f); //0.37
            contrastEffect.Parameters["alpha"].SetValue(0.5f);
            contrastEffect.Parameters["redVignetteActive"].SetValue(false);
            // Colors
            contrastEffect.Parameters["Contrast"].SetValue(0.2f);
            contrastEffect.Parameters["Brightness"].SetValue(0.05f);
            contrastEffect.Parameters["Hue"].SetValue(0f);
            contrastEffect.Parameters["Saturation"].SetValue(1.4f);
            postprocessor = new PostProcessing(contrastEffect, Hazmat.Instance.GraphicsDevice);

            SetInstance(this.renderCapture);
            SetInstance(this.postprocessor);
        }

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
            this.inputManager.Register(Keys.Q);

            // Powerup - Keyboard
            this.inputManager.Register(Keys.R); // Left
            this.inputManager.Register(Keys.T); // Right

            // GAMEPAD
            // Player - Gamepad 
            this.inputManager.Register(ThumbSticks.Left);

            // Shooting - Gamepad
            this.inputManager.Register(Buttons.RightTrigger);
            this.inputManager.Register(ThumbSticks.Right);

            // Interact - Gamepad
            this.inputManager.Register(Buttons.X);

            // Event - Keyboard
            this.inputManager.Register(Buttons.B);
            this.inputManager.Register(Buttons.A);

            // Powerup - Keyboard
            this.inputManager.Register(Buttons.LeftShoulder);
            this.inputManager.Register(Buttons.RightShoulder);
        }

    }
}

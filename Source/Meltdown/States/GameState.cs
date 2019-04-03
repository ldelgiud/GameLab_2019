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

        public override void Initialize(Game1 game)
        {
            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.E);
            this.SetInstance(this.inputManager);

            this.window = game.Window;

            Energy energy = new Energy();
            PowerPlant powerPlant = new PowerPlant();

            this.SetInstance(new QuadTree<Entity>(
                new AABB()
                {
                    LowerBound = new Vector2(-10000, -10000),
                    UpperBound = new Vector2(10000, 10000)
                },
                10, 7));

            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            this.worldCamera = new Camera2D(
                new Transform2D(),
                80,
                45
                );

            this.world = new World();
            this.SetInstance(this.world);

            // Resource Managers
            this.textureResourceManager = new TextureResourceManager(game.Content);
            this.textureResourceManager.Manage(this.world);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                new DebugCollisionHandler(this.world),
                new ProjectileCollisionHandler(this.world),
                new EnergyPickupCollisionHandler(this.world, energy),
                new EventTriggerCollisionHandler(this.world),
                new PlayerDroneCollisionHandler(this.world, energy)
            });

            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, this.GetInstance<QuadTree<Entity>>(), collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world);
            EventSystem eventSystem = new EventSystem(this.world);
            AISystem aISystem = new AISystem(this.world);
            PowerplantSystem powerplantSystem =
                new PowerplantSystem(this.world, energy, powerPlant);

            ShootingSystem shootingSystem = new ShootingSystem(world);
            CameraSystem cameraSystem = new CameraSystem(this.worldCamera, this.world);
            EnemySpawnSystem enemySpawnSystem = new EnemySpawnSystem();
            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                shootingSystem,
                collisionSystem,
                eventSystem,
                aISystem,
                powerplantSystem,
                cameraSystem
                );
            
            EnergyDrawSystem energyDrawSystem =
                new EnergyDrawSystem(
                    energy,
                    game.Content.Load<Texture2D>("placeholders/EnergyBar PLACEHOLDER"),
                    game.GraphicsDevice,
                    game.Content.Load<SpriteFont>("gui/EnergyFont")
                    );

            //AABBDebugDrawSystem aabbDebugDrawSystem = new AABBDebugDrawSystem(world, game.GraphicsDevice, this.worldCamera, game.Content.Load<Texture2D>("boxColliders"));

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.worldCamera, this.world),
                new ScreenTextureSystem(game.GraphicsDevice, this.screenCamera, this.world),
                energyDrawSystem
                //aabbDebugDrawSystem
                );




            // Create player
            SpawnHelper.SpawnPlayer(1);

            //Crete Powerplant
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant);

            //Spawn enemy
            SpawnHelper.SpawnEnemy(new Vector2(25, -25), true);

            //SpawnHelper.SpawnEnemy(new Vector2(-250, -250), false);

            // Create energy pickup
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, new Vector2(-20, 20));


            // Event trigger
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(0, -20);
                AABB aabb = new AABB(new Vector2(-5, -5), new Vector2(5, 5));
                Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;

                entity.Set(new Transform2DComponent() { value = new Transform2D(position) });
                entity.Set(new WorldSpaceComponent());
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, false));
                entity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"placeholder", null, null, new Vector2(0.1f, 0.1f))));
                entity.Set(new EventTriggerComponent(new StoryIntroEvent()));

                physicsSystem.quadtree.AddNode(element);
            }

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
    }
}

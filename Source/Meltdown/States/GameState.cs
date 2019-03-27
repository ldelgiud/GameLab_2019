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
        InputManager inputManager;
        Camera worldCamera;
        Camera screenCamera;
        World world;
        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        TextureResourceManager textureResourceManager;

        public override void Initialize(Game1 game)
        {
            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.E);
            this.SetInstance(this.inputManager);

            Energy energy = new Energy();
            PowerPlant powerPlant = new PowerPlant();

            this.SetInstance(new QuadTree<Entity>(
                new AABB()
                {
                    LowerBound = new Vector2(-10000, -10000),
                    UpperBound = new Vector2(10000, 10000)
                },
                10, 7));

            this.screenCamera = new Camera(
                new Transform(new Vector3(0, 0, -1)),
                Matrix.CreateOrthographic(1920, 1080, 0, 2)
                );

            this.worldCamera = new Camera(
                new Transform(new Vector3(0, 0, -1)),
                Matrix.CreateOrthographic(100, 100, 0, 2)
                );

            this.world = new World();
            this.SetInstance(this.world);
            this.textureResourceManager = new TextureResourceManager(game.Content);

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
                collisionSystem,
                eventSystem,
                shootingSystem,
                aISystem,
                powerplantSystem,
                enemySpawnSystem
                cameraSystem
                );
            
            EnergyDrawSystem energyDrawSystem =
                new EnergyDrawSystem(
                    energy,
                    game.Content.Load<Texture2D>("placeholders/EnergyBar PLACEHOLDER"),
                    game.GraphicsDevice,
                    game.Content.Load<SpriteFont >("gui/EnergyFont")
                    );

            AABBDebugDrawSystem aabbDebugDrawSystem = new AABBDebugDrawSystem(world, game.GraphicsDevice, camera, game.Content.Load<Texture2D>("boxColliders"));

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.camera, this.world),
                new ScreenTextureSystem(game.GraphicsDevice, this.world, this.screen),
                energyDrawSystem,
                aabbDebugDrawSystem
                );


            // Resource Managers
            this.textureResourceManager.Manage(this.world);

            // Create player
            SpawnHelper.SpawnPlayer(1);

            //Crete Powerplant
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant);

            //Spawn enemy
            //SpawnHelper.SpawnEnemy(new Vector2(250, -250), true);

            //SpawnHelper.SpawnEnemy(new Vector2(-250, -250), false);

            // Create energy pickup
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, new Vector2(-20, 20));


            // Event trigger
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(0, -20);
                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-5, -5),
                    UpperBound = new Vector2(5, 5)
                };
                Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;

                entity.Set(new WorldTransformComponent(new Transform(new Vector3(position, 0))));
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, false));
                entity.Set(new ManagedResource<string, Texture2D>(@"placeholder"));
                entity.Set(new BoundingBoxComponent(10, 10, 0));
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
            this.drawSystem.Update(time);
        }
    }
}

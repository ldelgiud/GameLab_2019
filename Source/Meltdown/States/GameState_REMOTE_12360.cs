using System.Diagnostics;

using Microsoft.Xna.Framework;
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
using Meltdown.Components.InputHandlers;
using Meltdown.ResourceManagers;
using Meltdown.Event;
using Meltdown.Utilities;

namespace Meltdown.States
{
    class GameState : State.State
    {
        Camera camera;
        World world;
        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        TextureResourceManager textureResourceManager;

        public override void Initialize(Game1 game)
        {
            Energy energy = new Energy();
            PowerPlant powerPlant = new PowerPlant();

            this.SetInstance(new QuadTree<Entity>(
                new AABB()
                {
                    LowerBound = new Vector2(-10000, -10000),
                    UpperBound = new Vector2(10000, 10000)
                },
                10, 7));

            this.camera = new Camera(game.Window, 1920, 1080);
            this.world = new World();
            this.SetInstance(this.world);
            this.textureResourceManager = new TextureResourceManager(game.Content);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                new DebugCollisionHandler(this.world),
                new EnergyPickupCollisionHandler(this.world, energy),
                new EventTriggerCollisionHandler(this.world),
            });
            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, this.GetInstance<QuadTree<Entity>>(), collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world);
            EventSystem eventSystem = new EventSystem(this.world);
            AISystem aISystem = new AISystem(this.world);
            PowerplantSystem powerplantSystem =
                new PowerplantSystem(this.world, energy, powerPlant);
            
            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                collisionSystem,
                eventSystem,
                aISystem,
                powerplantSystem
                );
            
            EnergyDrawSystem energyDrawSystem =
                new EnergyDrawSystem(
                    energy,
                    game.Content.Load<Texture2D>("placeholders/EnergyBar PLACEHOLDER"),
                    game.GraphicsDevice,
                    game.Content.Load<SpriteFont >("gui/EnergyFont")
                    );

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.camera, this.world),
                energyDrawSystem
                );


            // Resource Managers
            this.textureResourceManager.Manage(this.world);

            // Create player
            SpawnHelper.SpawnPLayer(1);

            //Crete Powerplant
            SpawnHelper.SpawnNuclearPowerPlant(powerPlant);
            //Spawn enemy
            SpawnHelper.SpawEnemy(new Vector2(250, 250), physicsSystem.quadtree);

            /* Create obstacle 
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(300, 300);
                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-50, -50),
                    UpperBound = new Vector2(50, 50)
                };
                Element<Entity> element = new Element<Entity>(aabb);
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;
                element.Value = entity;

                entity.Set(new WorldTransformComponent(position));
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, true));
                entity.Set(new ManagedResource<string, Texture2D>("placeholder"));
                entity.Set(new BoundingBoxComponent(100, 100, 0));

                physicsSystem.quadtree.AddNode(element);
            }*/

            // Create energy pickup
            SpawnHelper.SpawnBattery(Constants.BIG_BATTERY_SIZE, new Vector2(-300, 300));


            // Event trigger
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(0, -200);
                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-50, -10),
                    UpperBound = new Vector2(50, 10)
                };
                Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;

                entity.Set(new WorldTransformComponent(position));
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, false));
                entity.Set(new ManagedResource<string, Texture2D>(@"placeholder"));
                entity.Set(new BoundingBoxComponent(100, 20, 0));
                entity.Set(new EventTriggerComponent(new StoryIntroEvent()));

                physicsSystem.quadtree.AddNode(element);
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

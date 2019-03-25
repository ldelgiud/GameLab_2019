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
            this.camera = new Camera(game.Window, 1920, 1080);
            this.world = new World();
            this.textureResourceManager = new TextureResourceManager(game.Content);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                new DebugCollisionHandler(this.world),
                new EnergyPickupCollisionHandler(this.world),
                new EventTriggerCollisionHandler(this.world),
            });
            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world);
            EventSystem eventSystem = new EventSystem(this.world);
            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                collisionSystem,
                eventSystem
                );

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.camera, this.world)
                );


            // Resource Managers
            this.textureResourceManager.Manage(this.world);
  
            // Create player
            {
                //1
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(0, 0);
                Vector2 velocity = new Vector2(0, 0);

                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-50, -50),
                    UpperBound = new Vector2(50, 50)
                };
                Element<Entity> element = new Element<Entity>(aabb);
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;
                element.Value = entity;

                entity.Set(new PlayerComponent());
                entity.Set(new WorldTransformComponent(position));
                entity.Set(new VelocityComponent(velocity));
                entity.Set(new InputComponent(new InputHandlerPlayer(entity)));
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, true));
                entity.Set(new ManagedResource<string, Texture2D>("animIdle*100*13*84*94"));
                entity.Set(new BoundingBoxComponent(100f, 100f, 0f));
                physicsSystem.quadtree.AddNode(element);

                // Gun entity
                var gunEntity = this.world.CreateEntity();
                Vector2 localPosition = new Vector2(0, 0);
                
                WorldTransformComponent gunTransform = new WorldTransformComponent(
                    entity.Get<WorldTransformComponent>(), 
                    localPosition, 
                    0f, 
                    Vector2.One / 5
                    );
                gunEntity.Set(gunTransform);
                gunEntity.Set(new ManagedResource<string, Texture2D>("shooting/smallGun"));
                gunEntity.Set(new BoundingBoxComponent(100f, 100f, 0f));
                TextureComponent tex = gunEntity.Get<TextureComponent>();


            }

            // Create obstacle
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
            }

            // Create energy pickup
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(-300, 300);
                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-10, -10),
                    UpperBound = new Vector2(10, 10)
                };
                Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;

                entity.Set(new WorldTransformComponent(position));
                entity.Set(new AABBComponent(physicsSystem.quadtree, aabb, element, false));
                entity.Set(new ManagedResource<string, Texture2D>(@"placeholders\battery"));
                entity.Set(new BoundingBoxComponent(20, 20, 0));
                entity.Set(new EnergyPickupComponent(10));

                physicsSystem.quadtree.AddNode(element);
            }

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

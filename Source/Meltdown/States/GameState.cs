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
using Meltdown.Utilities;

namespace Meltdown.States
{
    class GameState : State.State
    {
        World world;
        ISystem<Time> updateSystem;
        ISystem<Time> drawSystem;

        TextureResourceManager textureResourceManager;

        public override void Initialize(Game1 game)
        {
            this.world = new World();
            this.textureResourceManager = new TextureResourceManager(game.Content);

            CollisionSystem collisionSystem = new CollisionSystem(new CollisionHandler[] {
                new DebugCollisionHandler(this.world)
            });
            PhysicsSystem physicsSystem = new PhysicsSystem(this.world, collisionSystem);
            InputSystem inputSystem = new InputSystem(this.world);
            this.updateSystem = new SequentialSystem<Time>(
                inputSystem,
                physicsSystem,
                collisionSystem
                );

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(this.world, game.spriteBatch)
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
                    LowerBound = new Vector2(0, -100),
                    UpperBound = new Vector2(100, 0)
                };
                Element<Entity> element = new Element<Entity>(aabb);
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;
                element.Value = entity;

                entity.Set(new WorldTransformComponent(position));
                entity.Set(new VelocityComponent(velocity));
                entity.Set(new InputComponent(new InputHandlerPlayer(entity)));
                entity.Set(new AABBComponent(aabb, element));
                entity.Set(new ManagedResource<string, Texture2D>("placeholder"));

                physicsSystem.quadtree.AddNode(element);
            }

            // Create obstacle
            {
                var entity = this.world.CreateEntity();

                Vector2 position = new Vector2(300, 300);
                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(0, -100),
                    UpperBound = new Vector2(100, 0)
                };
                Element<Entity> element = new Element<Entity>(aabb);
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;
                element.Value = entity;

                entity.Set(new WorldTransformComponent(position));
                entity.Set(new AABBComponent(aabb, element));
                entity.Set(new ManagedResource<string, Texture2D>("placeholder"));

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

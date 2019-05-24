﻿using System.Diagnostics;

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
using Meltdown.ResourceManagers;
using Meltdown.Utilities;

namespace Meltdown.States
{
    class TransformsTestGameState : State.State
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
                new DebugCollisionHandler(this.world)
            });
            PhysicsSystem physicsSystem = null;
            this.updateSystem = new SequentialSystem<Time>(
                physicsSystem,
                collisionSystem
                );

            this.drawSystem = new SequentialSystem<Time>(
                new TextureDrawSystem(game.GraphicsDevice, this.camera, this.world)
                );


            // Resource Managers
            this.textureResourceManager.Manage(this.world);

            // Tests
            Test3();


        }

        public override IStateTransition Update(Time gameTime)
        {
            this.updateSystem.Update(gameTime);
            return null;
        }

        public override void Draw(Time gameTime)
        {
            this.drawSystem.Update(gameTime);
        }


        // Test Scenes For TransformComponents
        public void Test1()
        {
            //1
            var entity = this.world.CreateEntity();
            Vector2 position = new Vector2(200, 200);

            WorldTransformComponent pTr = new WorldTransformComponent(position, MathHelper.Pi / 4, Vector2.One * 2);
            entity.Set(pTr);
            entity.Set(new ManagedResource<string, Texture2D>("placeholder"));

            //2
            var entity2 = this.world.CreateEntity();
            Vector2 position2 = new Vector2(100, 0);
            entity2.Set(new WorldTransformComponent(pTr, position2, MathHelper.Pi / 6, Vector2.One / 2));
            entity2.Set(new ManagedResource<string, Texture2D>("placeholder"));
        }

        public void Test2()
        {
            //1
            var entity = this.world.CreateEntity();
            Vector2 position = new Vector2(500, 500);
            Vector2 velocity = new Vector2(30, 30);
            WorldTransformComponent pTr = new WorldTransformComponent(position, MathHelper.Pi/4, Vector2.One);
            entity.Set(pTr);
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new ManagedResource<string, Texture2D>("placeholder"));
            
            //2
            var entity2 = this.world.CreateEntity();
            Vector2 position2 = new Vector2(-200, 0);
            entity2.Set(new WorldTransformComponent(pTr, position2, MathHelper.Pi / 6, Vector2.One));
            entity2.Set(new ManagedResource<string, Texture2D>("placeholder"));
        }

      
        public void Test3()
        {
            //1
            var entity = this.world.CreateEntity();
            Vector2 position = new Vector2(0, 0);
            Vector2 velocity = new Vector2(30, 30);

            WorldTransformComponent pTr = new WorldTransformComponent(position, MathHelper.Pi/8, Vector2.One * 1.05f);
            entity.Set(pTr);
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new ManagedResource<string, Texture2D>("placeholder"));

            //2
            var entity2 = this.world.CreateEntity();
            Vector2 position2 = new Vector2(200, 0);
            entity2.Set(new WorldTransformComponent(pTr, position2, 0, Vector2.One));
            entity2.Set(new ManagedResource<string, Texture2D>("placeholder"));

            //3
            var entity3 = this.world.CreateEntity();
            Vector2 position3 = new Vector2(400, 100);
            entity3.Set(new WorldTransformComponent(entity2.Get<WorldTransformComponent>(), position3, MathHelper.Pi / 6, Vector2.One /2));
            entity3.Set(new ManagedResource<string, Texture2D>("placeholder"));

            //3
            var entity4 = this.world.CreateEntity();
            Vector2 position4 = new Vector2(200, 100);
            entity4.Set(new WorldTransformComponent(entity3.Get<WorldTransformComponent>(), position4, MathHelper.Pi / 6, Vector2.One * 3));
            entity4.Set(new ManagedResource<string, Texture2D>("placeholder"));

        }

    }
}
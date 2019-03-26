﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Components;
using Meltdown.AI;
using Meltdown.States;

using DefaultEcs;
using tainicom.Aether.Physics2D.Collision;
using Meltdown.Components.InputHandlers;
using DefaultEcs.Resource;

namespace Meltdown.Utilities
{
    class SpawnHelper
    {
        public static World World
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<World>();
            }
        }
        
        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            }
        }

        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static void SpawnPLayer(int playerID)
        {
            var entity = SpawnHelper.World.CreateEntity();

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

            entity.Set(new PlayerComponent(playerID));
            entity.Set(new WorldTransformComponent(position));
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new InputComponent(new InputHandlerPlayer(entity)));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new ManagedResource<string, Texture2D>("animIdle*100*13*84*94"));
            entity.Set(new BoundingBoxComponent(100f, 100f, 0f));
            SpawnHelper.quadtree.AddNode(element);

            // Gun entity
            var gunEntity = SpawnHelper.World.CreateEntity();
            Vector2 localPosition = new Vector2(0, 0);

            WorldTransformComponent gunTransform = new WorldTransformComponent(
                entity.Get<WorldTransformComponent>(),
                localPosition,
                0f,
                Vector2.One / 5
                );

            Texture2D bulletTexture = Game1.Instance.Content.Load<Texture2D>("shooting/bullet");
            gunEntity.Set(gunTransform);
            gunEntity.Set(new SmallGunComponent(35f, 500f, -1f, 0.1f, bulletTexture));
            gunEntity.Set(new ManagedResource<string, Texture2D>("shooting/smallGun"));
            gunEntity.Set(new BoundingBoxComponent(100f, 100f, 0f));

        }
        
        /// <summary>
        /// Spawn Nuclear Power Plant with all entities and attach respective components
        /// </summary>
        /// <param name="plant">Powerlplant object</param>
        public static void SpawnNuclearPowerPlant(PowerPlant plant)
        {
            var entity = SpawnHelper.World.CreateEntity();

            //Generate random position
            double angle = Constants.RANDOM.NextDouble() * Math.PI / 2.0;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            plant.Position = position;

            //Bounding box stuff
            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-100, -100),
                UpperBound = new Vector2(100, 100)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            //Create entity and attach the components to it
            entity.Set(new WorldTransformComponent(position));
            entity.Set(new ManagedResource<string, Texture2D>(@"placeholders\NuclearPlantPLACEHOLDER"));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new BoundingBoxComponent(200, 200, 0));

            SpawnHelper.quadtree.AddNode(element);
        }

        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="energy">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        public static void SpawnBattery(uint energy, Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-10, -10),
                UpperBound = new Vector2(10, 10)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            entity.Set(new WorldTransformComponent(position));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new ManagedResource<string, Texture2D>(@"placeholders\battery"));
            entity.Set(new BoundingBoxComponent(20, 20, 0));
            entity.Set(new EnergyPickupComponent(energy));

            SpawnHelper.quadtree.AddNode(element);
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(Vector2 position, QuadTree<Entity> quadtree)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-50, -50),
                UpperBound = new Vector2(50, 50)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            //Create entity and attach its components
            entity.Set(new WorldTransformComponent(position));
            entity.Set(new VelocityComponent(new Vector2(0, 0)));
            entity.Set(new HealthComponent(100));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new ManagedResource<string, Texture2D>("placeholder"));
            entity.Set(new BoundingBoxComponent(50, 50, 0));
            entity.Set(new AIComponent(new StandbyState()));
            SpawnHelper.quadtree.AddNode(element);

        }
    }
}


using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Components;
using Meltdown.AI;
using Meltdown.States;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;

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
        public static void SpawnPlayer(int playerID)
        {
            var entity = SpawnHelper.World.CreateEntity();

            Vector2 position = new Vector2(0, 0);
            Vector2 velocity = new Vector2(0, 0);

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-1, -1),
                UpperBound = new Vector2(1, 1)
            };
            Element<Entity> element = new Element<Entity>(aabb);
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;
            element.Value = entity;

            entity.Set(new PlayerComponent(playerID));
            entity.Set(new Transform2DComponent(new Transform2D(position)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new InputComponent(new InputHandlerPlayer(entity)));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"player1 PLACEHOLDER", null, null, 2.0f, 2.0f)));
            SpawnHelper.quadtree.AddNode(element);

            // Gun entity
            var gunEntity = SpawnHelper.World.CreateEntity();
            Vector2 localPosition = new Vector2(0, 0);

            var gunTransform = new Transform2DComponent(
                new Transform2D(
                    localPosition,
                    0,
                    Vector2.One / 5,
                    entity.Get<Transform2DComponent>().value
                    )
                );

            gunEntity.Set(gunTransform);
            gunEntity.Set(new WorldSpaceComponent());
            gunEntity.Set(new SmallGunComponent(35f, 25f, -1f, 0.1f));
            gunEntity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo("shooting/smallGun", null, null, 0.5f, 0.2f)));

        }
        
        /// <summary>
        /// Spawn Nuclear Power Plant with all entities and attach respective components
        /// </summary>
        /// <param name="plant">Powerlplant object</param>
        public static void SpawnNuclearPowerPlant(PowerPlant plant)
        {
            var entity = SpawnHelper.World.CreateEntity();

            //Generate random position
            double angle = Constants.RANDOM.NextDouble() * MathHelper.PiOver2;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            plant.Position = position;

            //Bounding box stuff
            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-5, -5),
                UpperBound = new Vector2(5, 5)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            //Create entity and attach the components to it
            entity.Set(new Transform2DComponent(new Transform2D(position)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"placeholders\NuclearPlantPLACEHOLDER", null, null, 10f, 10f)));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));

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
                LowerBound = new Vector2(-0.5f, -0.5f),
                UpperBound = new Vector2(0.5f, 0.5f)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            entity.Set(new Transform2DComponent(new Transform2D(position)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"placeholders\battery", null, null, 1.0f, 1.0f)));
            entity.Set(new EnergyPickupComponent(energy));

            SpawnHelper.quadtree.AddNode(element);
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawnEnemy(Vector2 position, bool drone)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-1, -1),
                UpperBound = new Vector2(1, 1)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            
            //Create entity and attach its components
            entity.Set(new Transform2DComponent(new Transform2D(position)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new VelocityComponent(new Vector2(0, 0)));
            entity.Set(new HealthComponent(100));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            if (drone)
            {
                entity.Set(new AIComponent(new DroneStandby()));
                entity.Set(new ManagedResource<Texture2DInfo,
                    Texture2D>(new Texture2DInfo(@"placeholders/enemies/drone", null, MathF.PI / 2, 2.0f, 2.0f)));
                entity.Set(new DroneComponent(200));
            }
            else
            {
                entity.Set(new AIComponent(new ShooterStandby()));
                entity.Set(new ManagedResource<Texture2DInfo,
                    Texture2D>(new Texture2DInfo(@"placeholder", null, null, 2.0f, 2.0f)));
            }
            SpawnHelper.quadtree.AddNode(element);
        }

        public static void SpawnRandomEnemy(bool drone)
        {
            var entity = SpawnHelper.World.CreateEntity();
            AABB aabb;
            bool collides;
            Vector2 position;
            Element<Entity> element;
            /*do
            {*/
                collides = false;
                position = new Vector2(
                Constants.RANDOM.Next(-600, 600),
                Constants.RANDOM.Next(-500, 500));
                aabb = new AABB()
                {
                    LowerBound = new Vector2(-50, -50),
                    UpperBound = new Vector2(50, 50)
                };
                element = new Element<Entity>(aabb) { Value = entity };
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;
                /*
                SpawnHelper.quadtree.QueryAABB((Element<Entity> collidee) =>
                {
                    if (collidee == element)
                    {
                        return true;
                    }
                    else
                    {
                        collides = true;
                        return false;
                    }
                }, ref aabb);

            } while (collides);*/
            

            //Create entity and attach its components
            entity.Set(new Transform2DComponent(new Transform2D(position)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new VelocityComponent(new Vector2(0, 0)));
            entity.Set(new HealthComponent(100));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            if (drone)
            {
                entity.Set(new AIComponent(new DroneStandby()));
                entity.Set(new ManagedResource<Texture2DInfo,
                    Texture2D>(new Texture2DInfo(@"placeholders/enemies/drone", null, null, 2.0f, 2.0f)));
                entity.Set(new DroneComponent(200));
            }
            else
            {
                entity.Set(new AIComponent(new ShooterStandby()));
                entity.Set(new ManagedResource<Texture2DInfo,
                    Texture2D>(new Texture2DInfo(@"placeholder", null, null, 2.0f, 2.0f)));
            }
            SpawnHelper.quadtree.AddNode(element);

        }
    }
}


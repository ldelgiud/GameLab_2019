using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Meltdown.Components;
using Meltdown.AI;
using Meltdown.States;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;
using Meltdown.Components.InputHandlers;

using DefaultEcs;
using DefaultEcs.Resource;

using tainicom.Aether.Physics2D.Collision;

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

        // World Camera
        public static Camera WorldCamera
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<Camera>();
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
                LowerBound = new Vector2(-5f, -5f),
                UpperBound = new Vector2(5f, 5f)
            };
            Element<Entity> element = new Element<Entity>(aabb);
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;
            element.Value = entity;

            entity.Set(new PlayerComponent(playerID, 20));
            entity.Set(new AllianceMaskComponent(Alliance.Player));
            entity.Set(new WorldTransformComponent(new Transform(position.ToVector3(), Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f))));
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new InputComponent(new PlayerInputHandler()));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            //entity.Set(new ManagedResource<string, Texture2D>("animIdle*100*13*84*94"));
            entity.Set(new ManagedResource<string, ModelWrapper>(@"test\player"));
            entity.Set(new BoundingBoxComponent(2, 2, 0f));
            entity.Set(new NameComponent() { name = "player" });
            SpawnHelper.quadtree.AddNode(element);

            Entity gun = SpawnHelper.SpawnGun(entity);
            gun.Set(new InputComponent(new ShootingInputHandler(World)));

        }
        /// <summary>
        /// Assuming parent has WorldTransformComponent and AllianceMaskComponent
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Entity SpawnGun(Entity parent) {
            // Gun entity
            var gunEntity = SpawnHelper.World.CreateEntity();
            Vector2 localPosition = new Vector2(0, 0);

            WorldTransformComponent gunTransform = new WorldTransformComponent(
                new Transform(
                    position: localPosition.ToVector3(),
                    rotation: Vector3.Zero,
                    scale: Vector3.One / 5,
                    parent: parent.Get<WorldTransformComponent>().value
                    )
                );
            gunEntity.Set(gunTransform);

            Alliance alliance = parent.Get<AllianceMaskComponent>().alliance;
            gunEntity.Set(new SmallGunComponent(
                damage : 35f, 
                projectileSpeed : 25f, 
                radiusRange : -1f, 
                reloadTime : 0.5f, 
                projTex : "shooting/bullet", 
                alliance : alliance));
            gunEntity.Set(new ManagedResource<string, Texture2D>("shooting/smallGun"));
            gunEntity.Set(new BoundingBoxComponent(1f, 1f, 0f));
            gunEntity.Set(new NameComponent() { name = "gun" });
            gunEntity.SetAsChildOf(parent);
            parent.Set(new WeaponComponent(gunEntity));
            return gunEntity;
        }
        
        /// <summary>
        /// Spawn Nuclear Power Plant with all entities and attach respective components
        /// </summary>
        /// <param name="plant">Powerlplant object</param>
        public static void SpawnNuclearPowerPlant(PowerPlant plant)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = "powerplant" });

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
            entity.Set(new WorldTransformComponent(new Transform(position.ToVector3())));
            entity.Set(new ManagedResource<string, Texture2D>(@"placeholders\NuclearPlantPLACEHOLDER"));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new BoundingBoxComponent(10, 10, 0));

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

            entity.Set(new WorldTransformComponent(new Transform(position.ToVector3())));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new ManagedResource<string, Texture2D>(@"placeholders\battery"));
            entity.Set(new BoundingBoxComponent(1, 1, 0));
            entity.Set(new EnergyPickupComponent(energy));
            entity.Set(new NameComponent() { name = "battery" });

            SpawnHelper.quadtree.AddNode(element);
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        private static Entity SpawnBasicEnemy(Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-1f, -1f),
                UpperBound = new Vector2(1f, 1f)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;
            SpawnHelper.quadtree.AddNode(element);

            //Create entity and attach its components
            entity.Set(new AllianceMaskComponent(Alliance.Hostile));
            entity.Set(new WorldTransformComponent(new Transform(position.ToVector3())));
            entity.Set(new VelocityComponent(new Vector2(0, 0)));
            entity.Set(new HealthComponent(100));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new BoundingBoxComponent(2, 2, 0));
            entity.Set(new NameComponent() { name = "enemy" });
            return entity;
        }

        public static void SpawnShooter(Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);

            entity.Set(new AIComponent(new ShooterStandby()));
            entity.Set(new ManagedResource<string, Texture2D>("placeholder"));
            entity.Set(new NameComponent() { name = "shooter" });
            SpawnHelper.SpawnGun(entity);
        }
        
        public static void SpawnDrone (Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);

            entity.Set(new AIComponent(new DroneStandby()));
            entity.Set(new ManagedResource<string,
                Texture2D>("placeholders/enemies/drone"));
            entity.Set(new DamageComponent(200f));
            entity.Set(new NameComponent() { name = "drone" });
        }

        public static void SpawnRandomEnemy(bool drone, Vector2 seed, int range)
        {
            bool collides;
            int sanityCheck = 0;
            Vector2 position;
            do
            {
                collides = false;
                position = seed + new Vector2(
                Constants.RANDOM.Next(-range, range),
                Constants.RANDOM.Next(-range, range));

                AABB aabb = new AABB()
                {
                    LowerBound = new Vector2(-3f, -3f),
                    UpperBound = new Vector2(3f, 3f)
                };
                Element<Entity> element = new Element<Entity>(aabb);
                element.Span.LowerBound += position;
                element.Span.UpperBound += position;

                SpawnHelper.quadtree.QueryAABB((Element<Entity> collidee) =>
                {
                    AABBComponent collideeAABB = collidee.Value.Get<AABBComponent>();
                    if (!collideeAABB.solid)
                    {
                        return true;
                    }
                    collides = true;
                    return false;
                }, ref element.Span);

            } while (collides && (++sanityCheck < 20));

            if (collides) return;

            if (drone) SpawnHelper.SpawnDrone(position);
            else SpawnHelper.SpawnShooter(position);

        }

        public static void SpawnEnemyCamp(Vector2 position) 
        {
            int enemyCount = Constants.RANDOM.Next(5, 8);

            for (int i = 0; i < enemyCount; ++i)
            {
                bool drone = Constants.RANDOM.Next(3) == 0;
                SpawnHelper.SpawnRandomEnemy(drone, position, 50);
            }
        }

        public static Entity SpawnBullet(Vector3 position, Vector2 direction)
        {
            var entity = SpawnHelper.World.CreateEntity();

            float rotation = MathF.Atan2(-direction.Y, direction.X);
            var projectileTransform = new WorldTransformComponent(
                new Transform(
                    position,
                    Vector3.Zero,
                    Vector3.One * 0.05f
                    )
                );
            projectileTransform.value.Rotate(0, 0, rotation + MathHelper.PiOver2);
            entity.Set(projectileTransform);

            var aabb = new AABB(new Vector2(-0.1f, -0.1f), new Vector2(0.1f, 0.1f));
            var element = new Element<Entity>(aabb);
            element.Span.LowerBound += position.ToVector2();
            element.Span.UpperBound += position.ToVector2();
            element.Value = entity;
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            SpawnHelper.quadtree.AddNode(element);

            return entity;
        }
    }
}


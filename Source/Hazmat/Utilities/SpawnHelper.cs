﻿using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hazmat.Components;
using Hazmat.AI;
using Hazmat.States;
using Hazmat.Graphics;
using Hazmat.Utilities.Extensions;
using Hazmat.Components.InputHandlers;

using DefaultEcs;
using DefaultEcs.Resource;

using tainicom.Aether.Physics2D.Collision;
using Hazmat.Event;
using System.Collections.Generic;

namespace Hazmat.Utilities
{
    class SpawnHelper
    {
        public static World World
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<World>();
            }
        }
        
        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            }
        }

        public static TileMap TileMap
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<TileMap>();
            }
        }

        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static void SpawnPlayer(int playerID)
        {
            var entity = SpawnHelper.World.CreateEntity();

            Vector2 position = new Vector2(-10, -10);
            Vector2 velocity = new Vector2(0, 0);

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-1f, -1f),
                UpperBound = new Vector2(1f, 1f)
            };
            Element<Entity> element = new Element<Entity>(aabb);
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;
            element.Value = entity;

            AABB testAABB = aabb;
            testAABB.LowerBound += position;
            testAABB.UpperBound += position;
            List<Entity> entities = SpawnHelper.CollisionCheck(testAABB, true);
            foreach (Entity ent in entities)
            {
                ent.Delete();
            }

            var transform = new Transform3D(position.ToVector3());

            entity.Set(new PlayerComponent(playerID));
            entity.Set(new StatsComponent(Constants.PLAYER_SPEED, 0));
            entity.Set(new AllianceMaskComponent(Alliance.Player));
            entity.Set(new Transform3DComponent(transform));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new VelocityComponent(velocity));
            entity.Set(new InputComponent(new PlayerInputHandler()));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_PlayerMat_01",
                @"characters\MAT_CH_PlayerMat_01",
                rotation: new Vector3(0, 0, MathF.PI / 2),
                scale: new Vector3(0.07f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.5f) }
                )));
            entity.Set(new NameComponent() { name = "player" });
            SpawnHelper.quadtree.AddNode(element);

            {
                var maskEntity = SpawnHelper.World.CreateEntity();
                maskEntity.SetAsChildOf(entity);

                maskEntity.Set(new NameComponent() { name = "player_mask" });
                maskEntity.Set(new Transform3DComponent(new Transform3D(parent: transform)));
                maskEntity.Set(new WorldSpaceComponent());
                maskEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                    @"characters\armor\MED_AR_MatMask_01",
                    @"characters\armor\TEX_AR_TanksMasksBP_01",
                    rotation: new Vector3(0, 0, MathF.PI / 2),
                    scale: new Vector3(0.07f),
                    standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                    standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.5f) }
                )));
            }

            {
                var maskEntity = SpawnHelper.World.CreateEntity();
                maskEntity.SetAsChildOf(entity);

                maskEntity.Set(new NameComponent() { name = "player_backpack" });
                maskEntity.Set(new Transform3DComponent(new Transform3D(parent: transform)));
                maskEntity.Set(new WorldSpaceComponent());
                maskEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                    @"characters\armor\MED_AR_MatBackpack_01",
                    @"characters\armor\TEX_AR_TanksMasksBP_01",
                    rotation: new Vector3(0, 0, MathF.PI / 2),
                    scale: new Vector3(0.07f),
                    standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                    standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.5f) }
                )));
            }

            entity.SetModelAnimation("Take 001");

            SpawnHelper.SpawnCollectableGun(new Vector3(-5, -10, 2));
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

            var gunTransform = new Transform2DComponent(new Transform2D(localPosition, parent: parent.Get<Transform2DComponent>().value));

            gunEntity.Set(gunTransform);
            gunEntity.Set(new WorldSpaceComponent());

            Alliance alliance = parent.Get<AllianceMaskComponent>().alliance;
            gunEntity.Set(new SmallGunComponent(
                damage : 35f, 
                projectileSpeed : 25f, 
                radiusRange : -1f, 
                reloadTime : 0.5f, 
                projTex : "shooting/bullet", 
                alliance : alliance));
            gunEntity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo("shooting/smallGun", 1f, 0.4f)));
            gunEntity.Set(new NameComponent() { name = "gun" });
            gunEntity.SetAsChildOf(parent);
            parent.Set(new WeaponComponent(gunEntity));
            return gunEntity;
        }

        /// <summary>
        /// Spawns a collectable gun without an initial parent.
        /// </summary>
        public static Entity SpawnCollectableGun(Vector3 pos)
        {
            // Gun entity
            var gunEntity = SpawnHelper.World.CreateEntity();
            Vector3 localPosition = pos;

            Transform3DComponent gunTransform = new Transform3DComponent(new Transform3D(position: localPosition));
            gunEntity.Set(gunTransform);
            gunEntity.Set(new WorldSpaceComponent());

            gunEntity.Set(new SmallGunComponent(
                damage: 35f,
                projectileSpeed: 25f,
                radiusRange: -1f,
                reloadTime: 0.2f,
                projTex: "shooting/bullet",
                alliance: Alliance.Player));

            gunEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"weapons\MED_WP_MatGunBasic_01",
                @"weapons\TEX_WP_MatGunBasic_01",
                translation: new Vector3(0, 0, 0f),
                rotation: new Vector3(0, 0, MathF.PI / 2),
                scale: new Vector3(0.07f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                updateTimeEffect: true,
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("GlowLineThickness", 0.02f), new Tuple<string, float>("LineThickness", 0.02f) }
                )));

            gunEntity.Set(new NameComponent() { name = "gun" });
            gunEntity.Set(new InteractableComponent());
            gunEntity.Set(new PickUpGunComponent());
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
            double angle = Constants.RANDOM.NextDouble() * (Constants.MAX_RADIAN - Constants.MIN_RADIAN) + Constants.MIN_RADIAN;
            double x = Math.Round(Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle) / 10) * 10;
            //TODO: change this once camera work is done
            double y = Math.Round(Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle) / 10) * 10;
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
            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, 0))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_EN_Tile_PowerPlant_01", 14.14f, 8.165f)));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new PowerPlantComponent());
            entity.Set(new InteractableComponent());

            SpawnHelper.quadtree.AddNode(element);
        }

        public static void SpawnPlayerHouse()
        {
            SpawnHelper.SpawnHouse0(Vector2.Zero, 0);
        }

        public static void SpawnRandomHouse(Vector2 position)
        {
            int houseNr = Constants.RANDOM.Next(2);
            AABB aabb;
            if (houseNr == 0)
            {
                Vector2 lowerBound = new Vector2(position.X - 5, position.Y - 5);
                Vector2 upperBound = new Vector2(position.X + 5, position.Y + 5);
                aabb = new AABB(lowerBound, upperBound);
            } else
            {
                Vector2 lowerBound = new Vector2(position.X - 5, position.Y - 5);
                Vector2 upperBound = new Vector2(position.X + 5, position.Y + 10);
                aabb = new AABB(lowerBound, upperBound);
            }
            
           
            //Find correct rotation
            float DirToFace = 0;
            bool streetFound = false;
            for (int dist = 1; dist < 10; dist++)
            {
                for (int i = 0; i < 3; i++)
                {
                    AABB testAABB = new AABB()
                    {
                        LowerBound = new Vector2(-1f, -1f),
                        UpperBound = new Vector2(1f, 1f)
                    };
                    testAABB.LowerBound += position;
                    testAABB.UpperBound += position;

                    switch (i)
                    {
                        case 0:
                            testAABB.LowerBound += new Vector2(Constants.TILE_SIZE * dist, 0);
                            testAABB.UpperBound += new Vector2(Constants.TILE_SIZE * dist, 0);
                            break;
                        case 1:
                            testAABB.LowerBound += new Vector2(0, Constants.TILE_SIZE * dist);
                            testAABB.UpperBound += new Vector2(0, Constants.TILE_SIZE * dist);
                            break;
                        case 2:
                            testAABB.LowerBound -= new Vector2(Constants.TILE_SIZE * dist, 0);
                            testAABB.UpperBound -= new Vector2(Constants.TILE_SIZE * dist, 0);
                            break;
                        case 3:
                            testAABB.LowerBound -= new Vector2(0, Constants.TILE_SIZE * dist);
                            testAABB.UpperBound -= new Vector2(0, Constants.TILE_SIZE * dist);
                            break;
                        default:
                            break;

                    }
                    SpawnHelper.TileMap.quadtree.QueryAABB((Element<Entity> collidee) =>
                    {
                        if (collidee.Value.Has<NameComponent>())
                        {
                            if (collidee.Value.Get<NameComponent>().name == Constants.STREET_TILE_NAME)
                            {
                                DirToFace = (i+1) % 4;
                                streetFound = true;
                            }
                        }
                        return true;
                    }, ref testAABB);
                }
                if (streetFound) break;
            }

            AABB rotatedAABB = aabb.rotate((int)DirToFace);
            List<Entity> entities = SpawnHelper.CollisionCheck(rotatedAABB, true);
            if (entities.Count == 0)
            {
                if (houseNr == 0) SpawnHelper.SpawnHouse0(position, DirToFace);
                else SpawnHelper.SpawnHouse1(position, DirToFace);
            }
        }

        public static List<Entity> CollisionCheck(AABB aabb, bool solid)
        {
            List<Entity> entities = new List<Entity>();
            SpawnHelper.quadtree.QueryAABB((Element<Entity> collidee) =>
            {
                AABBComponent collideeAABB = collidee.Value.Get<AABBComponent>();
                if (collideeAABB.solid == solid)
                {
                    entities.Add(collidee.Value);
                }
                return true;
            }, ref aabb);

            return entities;
        }

        public static void SpawnHouse0(Vector2 position, float dirToFace)
        {
            var entity = SpawnHelper.World.CreateEntity();
            Vector3 rotation = new Vector3(Vector2.Zero, dirToFace * MathF.PI / 2);
            entity.Set(new NameComponent() { name = "House0" });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: rotation
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\houses\house1",
            @"buildings\houses\house1_tex",
            scale: new Vector3(3f),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
            standardEffectInitialize: new Tuple<string, float>[] {  new Tuple<string, float>("LineThickness", 0.04f) }
            )));

            entity.Set(new WorldSpaceComponent());

            SpawnHelper.AddAABB(entity, position, new Vector2(-5, -5), new Vector2(5, 5), true);
        }

        public static void SpawnHouse1(Vector2 position, float dirToFace)
        {

            var entity = SpawnHelper.World.CreateEntity();
            Vector3 rotation = new Vector3(Vector2.Zero, dirToFace * MathF.PI / 2);
            entity.Set(new NameComponent() { name = "House1" });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: rotation
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\houses\house2",
            @"buildings\houses\house2_tex",
            scale: new Vector3(3f),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
            standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.04f) }
            )));

            entity.Set(new WorldSpaceComponent());

            AABB aabb = new AABB(new Vector2(-5, -5), new Vector2(5, 10));
            AABB rotatedAABB = aabb.rotate((int)dirToFace);
            if (dirToFace == 1)
            {
                rotatedAABB.LowerBound =
                    new Vector2(rotatedAABB.LowerBound.X - 2, rotatedAABB.LowerBound.Y - 3);
                rotatedAABB.UpperBound =
                    new Vector2(rotatedAABB.UpperBound.X - 2, rotatedAABB.UpperBound.Y - 3);
            } else if (dirToFace == 2)
            {
                rotatedAABB.LowerBound =
                    new Vector2(rotatedAABB.LowerBound.X, rotatedAABB.LowerBound.Y - 5);
                rotatedAABB.UpperBound =
                    new Vector2(rotatedAABB.UpperBound.X, rotatedAABB.UpperBound.Y - 5);
            } else if (dirToFace == 3)
            {
                rotatedAABB.LowerBound =
                    new Vector2(rotatedAABB.LowerBound.X+3, rotatedAABB.LowerBound.Y-2);
                rotatedAABB.UpperBound =
                    new Vector2(rotatedAABB.UpperBound.X+3, rotatedAABB.UpperBound.Y-2);
            }
            Element<Entity> element = new Element<Entity>(rotatedAABB) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            SpawnHelper.quadtree.AddNode(element);
            entity.Set(new AABBComponent(SpawnHelper.quadtree, rotatedAABB, element, true));

            Debug.WriteLine("Position: " + position);
            Debug.WriteLine("AABB lowerBound: " + rotatedAABB.LowerBound );
            Debug.WriteLine("AABB upperBound: " + rotatedAABB.UpperBound);
        }

        public static void SpawnSidewalk(Vector2 position, Vector3 rotation)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = "Sidewalk" });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: rotation
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_01",
                scale: new Vector3(6f)
                )));
            entity.Set(new WorldSpaceComponent());

            AABB aabb = new AABB(position, 3, 3);
            List<Entity> entities = SpawnHelper.CollisionCheck(aabb, false);
            foreach (Entity ent in entities) ent.Delete();

            SpawnHelper.AddAABB(entity, position, new Vector2(-2.5f), new Vector2(2.5f), false);
        }
        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="energy">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        /// 

        public static void SpawnSidewalkWalls(Vector2 position)
        {


        }

        public static void SpawnMailBox(Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);
            entity.Remove<VelocityComponent>();
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_Mailbox_01",
                @"characters\mailbox_tex",
                scale: new Vector3(3f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] {
                    new Tuple<string, float>("LineThickness", 0.05f)}
                )));


        }
        
        /// <summary>
        /// Spawn an enemy entity at given position in offline state
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
            entity.Set(new Transform3DComponent(new Transform3D(position.ToVector3())));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AllianceMaskComponent(Alliance.Hostile));
            entity.Set(new VelocityComponent(Vector2.Zero));
            entity.Set(new HealthComponent(100));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new NameComponent() { name = "enemy" });
            return entity;
        }

        public static void SpawnPowerUp(Vector2 position)
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

            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, 0))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                new SpineAnimationInfo(
                    @"items\SPS_Collectables",
                    new SkeletonInfo(2f, 2f, skin: "chip_01", translation: new Vector3(0, 0, 0.1f)),
                    new AnimationStateInfo("chip_01", true)
                )
            ));

            entity.Set(new PowerUpComponent());
            entity.Set(new NameComponent() { name = "Power Up" });

            SpawnHelper.quadtree.AddNode(element);
        }

        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="energy">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        public static void SpawnBattery(uint energy, Vector2 position, Vector2? size = null)
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

            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, Constants.LAYER_FOREGROUND))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));

            float width = 2;
            float height = 2;
            if (size != null) { width = size.Value.X; height = size.Value.Y; }

            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                new SpineAnimationInfo(
                    @"items\SPS_Collectables",
                    new SkeletonInfo(width, height, skin: "battery_01", translation: new Vector3(0, 0, 0.5f)),
                    new AnimationStateInfo("battery_01", true)
                )
            ));
            entity.Set(new EnergyPickupComponent(energy));
            entity.Set(new NameComponent() { name = "battery" });

            SpawnHelper.quadtree.AddNode(element);
        }


        public static void SpawnLootStation(Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();

            // Create 2 more entities since we need 2 aabbs.
            Vector2 offset = new Vector2(3.3f, 0);
            
            // AABB Left Side
            var entityL = SpawnHelper.World.CreateEntity();
            AABB aabbL = new AABB()
            {
                LowerBound = new Vector2(-1f, -1f) * 0.8f,
                UpperBound = new Vector2(1f, 1f) * 0.8f
            };
            Element<Entity> elementL = new Element<Entity>(aabbL) { Value = entityL };
            elementL.Span.LowerBound += position - offset;
            elementL.Span.UpperBound += position - offset;
            SpawnHelper.quadtree.AddNode(elementL);

            // AABB Right Side
            var entityR = SpawnHelper.World.CreateEntity();
            AABB aabbR = new AABB()
            {
                LowerBound = new Vector2(-1f, -1f) * 0.8f,
                UpperBound = new Vector2(1f, 1f) * 0.8f
            };
            Element<Entity> elementR = new Element<Entity>(aabbL) { Value = entityR };
            elementR.Span.LowerBound += position + offset;
            elementR.Span.UpperBound += position + offset;
            SpawnHelper.quadtree.AddNode(elementR);

            entityL.Set(new AABBComponent(SpawnHelper.quadtree, aabbL, elementL, true));
            entityR.Set(new AABBComponent(SpawnHelper.quadtree, aabbR, elementR, true));
            

            // Add AABB Entities as childs of main enitity.
            entity.SetAsParentOf(entityL);
            entity.SetAsParentOf(entityR);


            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\environment\MES_EN_LootStation_01",
            @"buildings\environment\TEX_EN_LootStation_01",
            scale: new Vector3(0.1f),
            rotation: new Vector3(0, 0, MathHelper.Pi),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
            updateTimeEffect: true,
            standardEffectInitialize: new Tuple<string, float>[] {
                new Tuple<string, float>("GlowLineThickness", 1f),
                new Tuple<string, float>("LineThickness", 0.4f)}
            )));

            entity.Set(new InteractableComponent());
            entity.Set(new LootableComponent());
            entity.Set(new Transform3DComponent(new Transform3D(position.ToVector3())));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new NameComponent() { name = "loot_station" });
        }

        public static void SpawnLootBox(Vector2 position)
        {
            throw new Exception("Lootbox does not have a valid model currently");
            //var entity = SpawnHelper.World.CreateEntity();
            //entity.Set(new Transform2DComponent(new Transform2D(position)));
            //entity.Set(new WorldSpaceComponent());
            //entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"placeholders\lootbox2", width: 2f, height: 2f)));
            //entity.Set(new InteractableComponent());
            //entity.Set(new LootableComponent());
            //SpawnHelper.AddAABB(entity, position, new Vector2(-1, -1), new Vector2(1, 1), true);
        }

        public static void SpawnShooter(Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);

            entity.Set(new AIComponent(new ShooterOffline()));
            entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo("placeholder", 2, 2)));
            entity.Set(new NameComponent() { name = "shooter" });
            SpawnHelper.SpawnGun(entity);
        }
        
        public static void SpawnDrone (Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);

            entity.Set(new AIComponent(new DroneOffline()));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_EnemyBicycle_01",
                @"characters\TEX_CH_EnemyBicycle_01",
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"), 
                standardEffectInitialize: new Tuple<string, float>[] {
                    new Tuple<string, float>("LineThickness", 0.1f)}
                )));
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
                bool drone = true; //Constants.RANDOM.Next(3) == 0;
                SpawnHelper.SpawnRandomEnemy(drone, position, 50);
            }
        }

        public static Entity SpawnBullet(Vector3 position, Vector2 direction)
        {
            var entity = SpawnHelper.World.CreateEntity();

            // Since it is a texture the rotation is in World Space (I guess)
            float dirRot = -Camera2D.WorldToPerspective(direction).ToRotation() + MathHelper.Pi;

            var projectileTransform = new Transform3DComponent(new Transform3D(position, rotation: new Vector3(0,0,dirRot)));
            entity.Set(projectileTransform);
            entity.Set(new WorldSpaceComponent());

            var aabb = new AABB(new Vector2(-0.2f, -0.2f), new Vector2(0.2f, 0.2f));
            var element = new Element<Entity>(aabb);
            element.Span.LowerBound += position.ToVector2();
            element.Span.UpperBound += position.ToVector2();
            element.Value = entity;
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            SpawnHelper.quadtree.AddNode(element);

            return entity;
        }

        public static Entity SpawnEvent(Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB(new Vector2(-5, -5), new Vector2(5, 5));
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            entity.Set(new Transform2DComponent() { value = new Transform2D(position) });
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new EventTriggerComponent(new StoryIntroEvent()));
            entity.Set(new NameComponent() { name = "intro_event_trigger" });

            SpawnHelper.quadtree.AddNode(element);
            return entity;
        }

        public static void SpawnBasicWall(Vector2 center, float height, float width)
        {
            var entity = SpawnHelper.World.CreateEntity();

            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-width/2, -height/2),
                UpperBound = new Vector2( width/2,  height/2)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += center;
            element.Span.UpperBound += center;
            SpawnHelper.quadtree.AddNode(element);

            //Create entity and attach its components
            entity.Set(new Transform2DComponent(new Transform2D(center)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new NameComponent() { name = "Wall" });
        }

        public static void SpawnRoadBlock(Vector2 position, int direction)
        {

            var entity = SpawnHelper.World.CreateEntity();

            entity.Set(new NameComponent() { name = "RoadBlock" });

            if (direction == 0)

            {
                SpawnHelper.AddAABB(entity, position, new Vector2(-2, -5), new Vector2(2, 5), true);
            } else

            {

                SpawnHelper.AddAABB(entity, position, new Vector2(-5, -2), new Vector2(5, 2), true);

            }



            //Create entity and attach its components
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\barrier\barrier_01",
                @"buildings\barrier\barrier_01_tex",
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.02f) },
                scale: new Vector3(6f)
                )));
            entity.Set(new Transform3DComponent(new Transform3D(

                position: new Vector3(position, 0),

                rotation: new Vector3(Vector2.Zero, -(1-direction)*MathF.PI/2)

                )));

            entity.Set(new WorldSpaceComponent());
        }

        public static void AddAABB(Entity entity, Vector2 position, Vector2 lowerBound, Vector2 upperBound, bool solid)
        {
            AABB aabb = new AABB(lowerBound, upperBound);
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            SpawnHelper.quadtree.AddNode(element);
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, solid));
        }
        
    }
}


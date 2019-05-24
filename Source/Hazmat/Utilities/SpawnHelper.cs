using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hazmat.Components;
using Hazmat.AI;
using Hazmat.States;
using Hazmat.Graphics;
using Hazmat.Utilities.Extensions;
using Hazmat.Components.InputHandlers;
using Hazmat.Event;

using DefaultEcs;
using DefaultEcs.Resource;

using tainicom.Aether.Physics2D.Collision;
using System.Collections.Generic;
using Hazmat.AI.MailBoxAI;

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

        public static Street Street
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<Street>();
            }
        }

        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static void SpawnPlayer(int playerID, Vector2 position)
        {
            Debug.WriteLine("START: generation of player_0" + playerID);

            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.PLAYER_NAME + playerID });

            AABB testAABB = new AABB(position,2,2);
            List<Entity> entities = SpawnHelper.CollisionCheck(testAABB, true);
            foreach (Entity ent in entities)
            {
                ent.Delete();
            }
            SpawnHelper.AttachAABB(entity, position, 2, 2, true);

            var transform = new Transform3D(position.ToVector3());
            entity.Set(new PlayerComponent(playerID));
            //var playerStats = new StatsComponent(Constants.PLAYER_INITIAL_SPEED, 0, entity);
            //entity.Set(playerStats);
            entity.Set(new AllianceMaskComponent(Alliance.Player));
            entity.Set(new Transform3DComponent(transform));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new VelocityComponent());
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_PlayerMat_01",
                @"characters\MAT_CH_PlayerMat_01",
                rotation: new Vector3(0, 0, MathF.PI / 2),
                scale: new Vector3(0.07f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.5f) }
                )));

            {
                var maskEntity = SpawnHelper.World.CreateEntity();
                maskEntity.SetAsChildOf(entity);
                SpawnHelper.AttachAABB(maskEntity, Vector2.Zero, -Vector2.One, Vector2.One, false);
                maskEntity.Set(new AABBTetherComponent(entity));

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
                var backpackEntity = SpawnHelper.World.CreateEntity();
                backpackEntity.SetAsChildOf(entity);
                SpawnHelper.AttachAABB(backpackEntity, Vector2.Zero, -Vector2.One, Vector2.One, false);
                backpackEntity.Set(new AABBTetherComponent(entity));

                backpackEntity.Set(new NameComponent() { name = "player_backpack" });
                backpackEntity.Set(new Transform3DComponent(new Transform3D(parent: transform)));
                backpackEntity.Set(new WorldSpaceComponent());
                backpackEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                    @"characters\armor\MED_AR_MatBackpack_01",
                    @"characters\armor\TEX_AR_TanksMasksBP_01",
                    rotation: new Vector3(0, 0, MathF.PI / 2),
                    scale: new Vector3(0.07f),
                    standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                    standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.5f) }
                )));
            }

            {
                var gunEntity = SpawnHelper.World.CreateEntity();

                gunEntity.Set(new Transform3DComponent(new Transform3D(parent: transform, position: new Vector3(-1.15f, -1f, 0.25f))));
                gunEntity.Set(new WorldSpaceComponent());

                SpawnHelper.AttachAABB(gunEntity, Vector2.Zero, -Vector2.One, Vector2.One, false);
                gunEntity.Set(new AABBTetherComponent(parent: entity));
                gunEntity.SetAsChildOf(entity);

                gunEntity.Set(new SmallGunComponent(
                    damage: Constants.PLAYER_INITIAL_DAMAGE,
                    projectileSpeed: Constants.BULLET_SPEED,
                    radiusRange: -1f,
                    reloadTime: Constants.PLAYER_RELOAD_TIME,
                    projectileSkin: "MatProjectile_01",
                    projectileAnimation: "MatProjectile_01",
                    alliance: Alliance.Player));

                gunEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                    @"weapons\MED_WP_MatGunBasic_01",
                    @"weapons\TEX_WP_MatGunBasic_01",
                    translation: new Vector3(0, 0, 0f),
                    rotation: new Vector3(0, 0, MathF.PI),
                    scale: new Vector3(0.07f),
                    standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                    updateTimeEffect: true,
                    standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("GlowLineThickness", 0.02f), new Tuple<string, float>("LineThickness", 0.02f) }
                    )));


                gunEntity.Set(new NameComponent() { name = Constants.GUN_NAME });

                entity.Set(new WeaponComponent(gunEntity));

                var playerStats = new StatsComponent(Constants.PLAYER_INITIAL_SPEED, 0, entity, gunEntity.Get<ModelComponent>());
                entity.Set(playerStats);

                gunEntity.Set(new InputComponent(new ShootingInputHandler(SpawnHelper.World, playerStats)));

            }

            entity.SetModelAnimation("Take 001");

            var model = entity.Get<ModelComponent>();
            model.EnableDamageEffect(false);
            model.EnableDamageEffectForChildren(entity, false);

            Debug.WriteLine("END: generation of player_0" + playerID);
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

            // TODO: Remove AABB on pickup
            SpawnHelper.AttachAABB(gunEntity, pos.ToVector2(), -Vector2.One, Vector2.One, false);

            gunEntity.Set(new SmallGunComponent(
                damage: 35f,
                projectileSpeed: Constants.BULLET_SPEED,
                radiusRange: -1f,
                reloadTime: Constants.PLAYER_RELOAD_TIME,
                projectileSkin: "MatProjectile_01",
                projectileAnimation: "MatProjectile_01",
                alliance: Alliance.Player));

            gunEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"weapons\MED_WP_MatGunBasic_01",
                @"weapons\TEX_WP_MatGunBasic_01",
                translation: new Vector3(0, 0, 0f),
                rotation: new Vector3(0, 0, MathF.PI),
                scale: new Vector3(0.07f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                updateTimeEffect: true,
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("GlowLineThickness", 0.02f), new Tuple<string, float>("LineThickness", 0.02f) }
                )));

            gunEntity.Set(new NameComponent() { name = Constants.GUN_NAME });
            gunEntity.Set(new InteractableComponent());
            gunEntity.Set(new PickUpGunComponent());

            List<Entity> entities = SpawnHelper.CollisionCheck(gunEntity.Get<AABBComponent>().aabb, true);
            foreach (Entity ent in entities) ent.Delete();
            return gunEntity;
        }

        public static void SpawnRandomHouse(Vector2 position, int dir)
        {
            int houseNr = Constants.RANDOM.Next(2);
            //Find correct rotation
            if (dir == 100) dir = Street.FindClosestDirection(position);
            if ((houseNr == 0 && !SpawnHelper.IsCollisionFree(new AABB(position, 20, 20), true)) ||
                (houseNr == 1 && !SpawnHelper.IsCollisionFree(new AABB(position, 30, 20).rotate(dir), true)))
                   return;

            if (houseNr == 0) SpawnHelper.SpawnHouse0(position, dir);
            else SpawnHelper.SpawnHouse1(position, dir);

            bool spawnMailbox = Constants.RANDOM.Next(100) <= 60;
            if (spawnMailbox && position.Length()>=60)
            {
                Vector2 mailboxOffset = new Vector2(13, -13f).Rotate(dir * MathF.PI / 2);
                if (SpawnHelper.IsCollisionFree(new AABB(position + mailboxOffset, 2, 2), true))
                    SpawnMailBox(position + mailboxOffset);
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

        public static bool IsCollisionFree(AABB aabb, bool solid)
        {
            return SpawnHelper.CollisionCheck(aabb, solid).Count == 0;
        }

        public static void SpawnHouse0(Vector2 position, float dirToFace)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, new Vector2(-10f, -10f), new Vector2(10f, 10f), true);
            entity.Set(new NameComponent() { name = Constants.HOUSE_0_NAME });

            Vector3 rotation = new Vector3(Vector2.Zero, dirToFace * MathF.PI / 2);
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: rotation
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\houses\house1",
            @"buildings\houses\house1_tex",
            rotation: new Vector3(Vector2.Zero, MathF.PI/2),
            scale: new Vector3(6f),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
            standardEffectInitialize: new Tuple<string, float>[] {  new Tuple<string, float>("LineThickness", 0.04f) }
            )));

            entity.Set(new WorldSpaceComponent());

        }

        public static void SpawnHouse1(Vector2 position, float dirToFace)
        {
            //TODO: refactor
            var entity = SpawnHelper.World.CreateEntity();

            Vector3 rotation = new Vector3(Vector2.Zero, dirToFace * MathF.PI / 2);
            entity.Set(new NameComponent() { name = Constants.HOUSE_1_NAME });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: rotation
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\houses\house2_centered",
            @"buildings\houses\house2_tex",
            scale: new Vector3(6f),
            rotation: new Vector3(Vector2.Zero, MathF.PI/2),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
            standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.04f) }
            )));

            entity.Set(new WorldSpaceComponent());
            AABB aabb = new AABB(new Vector2(-15f,-10),new Vector2(15f,10)).rotate((int)dirToFace);
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            SpawnHelper.quadtree.AddNode(element);
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));

            //Debug.WriteLine("Position: " + position);
            //Debug.WriteLine("AABB lowerBound: " + rotatedAABB.LowerBound );
            //Debug.WriteLine("AABB upperBound: " + rotatedAABB.UpperBound);
        }

        public static void SpawnPlayerHouse()
        {
            Vector2 position = new Vector2(-25,0);
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, new Vector2(-15f, -10f), new Vector2(15f, 10f), true);
            entity.Set(new NameComponent() { name = Constants.PLAYER_HOUSE_NAME });

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
            @"buildings\houses\house3",
            @"buildings\houses\house3_tex",
            scale: new Vector3(6f),
            rotation: new Vector3(Vector2.Zero, MathF.PI/2),
            standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
            standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.04f) }
            )));

            entity.Set(new WorldSpaceComponent());
        }

        public static void SpawnSideWalk(Vector2 position, int dir)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, new Vector2(10/3), new Vector2(10/3), false);
            entity.Set(new NameComponent() { name = Constants.SIDEWALK_NAME });

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_01",
                @"buildings\sidewalk\sidewalk_01_tex",
                scale: new Vector3(8f)
                )));
            entity.Set(new WorldSpaceComponent());

            if (dir == 100)  return;

            Vector2 borderOffset = (new Vector2(Constants.TILE_SIZE / 6, 0)).Rotate(dir * MathF.PI / 2);
            SpawnHelper.SpawnSidewalkBorder(position + borderOffset, (dir+1)%4);
        }


        public static void SpawnRandomSuitcase(Vector2 position)
        {
            if (!SpawnHelper.IsCollisionFree(new AABB(position, 1, 1), true)) return;
            string suitcaseType = Constants.RANDOM.Next(1, 3) == 1 ? "A" : "B";
            float radian = (int)(Constants.RANDOM.NextDouble() * Math.PI * 2);

            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, new Vector2(1), new Vector2(1), true);
            entity.Set(new NameComponent() { name = Constants.SUITCASE_NAME + suitcaseType });

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, radian)
                )));

            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\suitcases\MES_EN_suitcase"+ suitcaseType +"_01",
                @"buildings\suitcases\TEX_EN_suitcases",
                scale: new Vector3(0.1f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 1f) }
                )));
        }

        public static void SpawnSidewalkBorder(Vector2 position, int dir)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.SIDEWALK_BORDER_NAME });

            if (dir == 1 || dir == 3) SpawnHelper.AttachAABB(entity, position, 1.5f, 20/3, false);
            else SpawnHelper.AttachAABB(entity, position, 20/3, 1.5f, false);

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, dir * MathF.PI / 2)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_border_01",
                @"buildings\sidewalk\sidewalk_border_barriers_01_tex",
                rotation: new Vector3(Vector2.Zero, MathF.PI/2),
                scale: new Vector3(8f)
                )));
            entity.Set(new WorldSpaceComponent());
        }

        public static void SpawnSidewalkBarrier(Vector2 position, int dir)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.SIDEWALK_BARRIER_NAME });

            if (dir == 1 || dir == 3) SpawnHelper.AttachAABB(entity, position, 4, 20, true);
            else SpawnHelper.AttachAABB(entity, position, 20, 4, true);

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, dir * MathF.PI / 2)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_barrier_01",
                @"buildings\sidewalk\sidewalk_border_barriers_01_tex",
                rotation: new Vector3(Vector2.Zero, MathF.PI / 2),
                scale: new Vector3(12f)
                )));
            entity.Set(new WorldSpaceComponent());
        }

        public static void SpawnSmallSidewalkBarrier(Vector2 position, int dir)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.SIDEWALK_BARRIER_NAME });

            if (dir == 1 || dir == 3) SpawnHelper.AttachAABB(entity, position, 4, 10, true);
            else SpawnHelper.AttachAABB(entity, position, 10, 4, true);

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, dir * MathF.PI / 2)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_barrier_02",
                @"buildings\sidewalk\sidewalk_border_barriers_01_tex",
                rotation: new Vector3(Vector2.Zero, MathF.PI / 2),
                scale: new Vector3(12f)
                )));
            entity.Set(new WorldSpaceComponent());
        }

        public static void SpawnTinySidewalkBarrier(Vector2 position, int dir)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.SIDEWALK_BARRIER_NAME });

            if (dir == 1 || dir == 3) SpawnHelper.AttachAABB(entity, position, 5, 6, true);
            else SpawnHelper.AttachAABB(entity, position, 6, 5, true);

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, dir * MathF.PI / 2)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\sidewalk\sidewalk_barrier_03",
                @"buildings\sidewalk\sidewalk_border_barriers_01_tex",
                rotation: new Vector3(Vector2.Zero, MathF.PI / 2),
                scale: new Vector3(12f)
                )));
            entity.Set(new WorldSpaceComponent());
        }

        public static void SpawnCompleteSidewalk(Vector2 position, int dir, bool wall)
        {
            float radian = dir * MathF.PI / 2;
            float SidewalkX = 2 * Constants.TILE_SIZE / 3;
            float SidewalkY = Constants.TILE_SIZE / 3;

            Vector2 sidewalk1 = (new Vector2(SidewalkX, SidewalkY)).Rotate(radian);
            Vector2 sidewalk2 = (new Vector2(SidewalkX, 0)).Rotate(radian);
            Vector2 sidewalk3 = (new Vector2(SidewalkX, -SidewalkY)).Rotate(radian);

            SpawnHelper.SpawnSideWalk(position + sidewalk1, (dir + 2) % 4);
            SpawnHelper.SpawnSideWalk(position + sidewalk2, (dir + 2) % 4);
            SpawnHelper.SpawnSideWalk(position + sidewalk3, (dir + 2) % 4);

            if (wall)
            {
                Vector2 BarrierOffset = (new Vector2(5*Constants.TILE_SIZE/6, 0)).Rotate(radian);
                SpawnHelper.SpawnSidewalkBarrier(position + BarrierOffset, (dir + 1) % 4);
            }
        }

        public static void SpawnLamp(Vector2 position, float radian)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, new Vector2(-0.25f), new Vector2(0.25f), true);
            entity.Set(new NameComponent() { name = Constants.LAMP_NAME });

            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, radian)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\lamp\lamp_01",
                @"buildings\lamp\lamp_tex",
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.02f) },
                scale: new Vector3(4f,4f,6f)
                )));

        }

        public static void SpawnRock(Vector2 position, int number)
        {
            if (!SpawnHelper.IsCollisionFree(new AABB(position, 6, 6), true)) return;
            var entity = SpawnHelper.World.CreateEntity();

            float radian = ((float)Constants.RANDOM.NextDouble()) * MathF.PI*2;
            string path = @"buildings\rock\rock_0" + number;
            string texpath = @"buildings\rock\rock_tex";
            SpawnHelper.AttachAABB(entity, position, new Vector2(-3f), new Vector2(3f), true);

            entity.Set(new NameComponent() { name = Constants.ROCK_NAME });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, radian)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                path,
                texpath,
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.02f) },
                scale: new Vector3(3f)
                )));

        }

        public static void SpawnBattery(uint size, Vector2 position)
        {

            float borderSize =  3f;
            if (size == Constants.MEDIUM_BATTERY_SIZE) borderSize = 4f;
            else if (size == Constants.BIG_BATTERY_SIZE) borderSize = 5f;
            
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, borderSize, borderSize, false);

            
            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, Constants.LAYER_FOREGROUND))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                new SpineAnimationInfo(
                    @"items\SPS_Collectables",
                    new SkeletonInfo(
                        width: borderSize, 
                        height: borderSize, 
                        skin: "battery_01", 
                        translation: new Vector3(0, 0, 1f)),
                    new AnimationStateInfo("battery_01", true)
                )
            ));
            entity.Set(new EnergyPickupComponent(size));
            entity.Set(new NameComponent() { name = Constants.BATTERY_NAME + size });

        }

        /// <summary>
        /// Spawn an enemy entity at given position in offline state
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        private static Entity SpawnBasicEnemy(Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, 3.5f, 3.5f,true);
            float offset = Math.Min((position.Length() / (float)Constants.PLANT_PLAYER_DISTANCE) * 200, 200)+10;
            int extraHealth = ((int)offset / 50) * 50;
            //Create entity and attach its components
            entity.Set(new Transform3DComponent(new Transform3D(position.ToVector3())));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new AllianceMaskComponent(Alliance.Hostile));
            entity.Set(new HealthComponent(100 + extraHealth));

            //TODO: NIHAT
            // Health Bar Entity
            //var healthBarEntity = SpawnHelper.World.CreateEntity();
            //healthBarEntity.Set(new Transform3DComponent(new Transform3D(position.ToVector3())));
            ////healthBarEntity.Set(new WorldSpaceComponent());
            //healthBarEntity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(
            //    new Texture2DInfo(
            //        @"static_sprites/SPT_UI_HUD_EnergyBack"
            //        //scale: new Vector2(10,10)
            //        )
            //    ));

            //healthBarEntity.SetAsChildOf(healthBarEntity);

            return entity;
        }

        public static void SpawnPowerUp(Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();
            int safetyCheck = 0;
            while (safetyCheck < 20 && !SpawnHelper.IsCollisionFree(new AABB(position, 3,3),true))
            {
                position += Vector2.One * 2.5f;
            } if (safetyCheck >= 20) return;

            SpawnHelper.AttachAABB(entity, position, 3, 3, false);

            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, 0))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                new SpineAnimationInfo(
                    @"items\SPS_Collectables",
                    new SkeletonInfo(3f, 5f, skin: "chip_01", translation: new Vector3(0, 0, 1)),
                    new AnimationStateInfo("chip_01", true)
                )
            ));

            entity.Set(new PowerUpComponent());
            entity.Set(new NameComponent() { name = Constants.POWERUP_NAME });
        }

        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="sized">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        public static void SpawnBattery(uint sized, Vector2 position, Vector2? size = null)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, position, 1, 1, false);
            entity.Set(new NameComponent() { name = Constants.BATTERY_NAME + sized });

            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, Constants.LAYER_FOREGROUND))));
            entity.Set(new WorldSpaceComponent());

            //TODO: NIHAT
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
            entity.Set(new EnergyPickupComponent(sized));

        }

        public static void SpawnCar(Vector2 position, int dir)
        {
            float radian = dir * MathF.PI / 2 + (float) Constants.RANDOM.NextDouble();
            int width = 10, height = 6;
            if (dir == 1 || dir == 3)
            {
                width = 6;
                height = 10;
            }
            if (!SpawnHelper.IsCollisionFree(new AABB(position, 1, 1), true)) return;
            
            var entity = SpawnHelper.World.CreateEntity();
            int number = Constants.RANDOM.Next(1, 3);
            string path = @"buildings\environment\car_0" + number;

            string texpath = @"buildings\environment\car_tex";
            int randomCarTex = Constants.RANDOM.Next(3);
            if (randomCarTex == 1) texpath += "_blue";
            if (randomCarTex == 2) texpath += "_purple";

            SpawnHelper.AttachAABB(entity, position, width, height, true);

            entity.Set(new NameComponent() { name = Constants.CAR_NAME + number });
            entity.Set(new Transform3DComponent(new Transform3D(
                position: new Vector3(position, 0),
                rotation: new Vector3(Vector2.Zero, radian)
                )));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                path,
                texpath,
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.02f) },
                scale: new Vector3(6f),
                rotation: new Vector3(Vector2.Zero, -MathF.PI/2)
                )));
        }

        public static void SpawnLootStation(Vector2 position, float dir)
        {
            var entity = SpawnHelper.World.CreateEntity();

            // Create 2 more entities since we need 2 aabbs.
            float radian = dir * MathF.PI / 2;
            Vector2 offset = new Vector2(3.3f, 0).Rotate(radian);
            // AABB Left Side
            var entityL = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entityL, position - offset, 1.6f, 1.6f, true);

            // AABB Right Side
            var entityR = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entityR, position + offset, 1.6f, 1.6f, true);
            
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

            // AABB for main entity, not solid
            SpawnHelper.AttachAABB(entity, position, 2, 2, false);

            entity.Set(new InteractableComponent());
            entity.Set(new LootableComponent());
            entity.Set(new Transform3DComponent(new Transform3D(
                position.ToVector3(),
                rotation: new Vector3(Vector2.Zero, radian))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new NameComponent() { name = Constants.LOOTING_STATION_NAME });
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

        //ENEMY SPAWNING
        public static void SpawnShooter(Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);
            entity.Set(new NameComponent() { name = Constants.SHOOTER_NAME });

            entity.Set(new VelocityComponent());
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_EnemyBicycle_01",
                @"characters\TEX_CH_EnemyBicycle_01",
                rotation: new Vector3(0, 0, -MathF.PI / 2),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] {
                    new Tuple<string, float>("LineThickness", 0.1f)}
            )));

            entity.Set(new SmallGunComponent(
                damage: Constants.MAILBOX_DAMAGE,
                projectileSpeed: Constants.BULLET_SPEED,
                radiusRange: -1f,
                reloadTime: Constants.MAILBOX_RELOAD_TIME,
                projectileSkin: "EnemyProjectile_01",
                projectileAnimation: "EnemyProjectile_01",
                alliance: Alliance.Hostile));
            entity.Set(new AIComponent(new ShooterOffline(entity)));
            entity.Set(new NameComponent() { name = Constants.SHOOTER_NAME });
        }
        
        public static void SpawnKamikaze (Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);
            entity.Set(new NameComponent() { name = Constants.DRONE_NAME });

            entity.Set(new VelocityComponent());
            entity.Set(new AIComponent(new DroneOffline(entity)));
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\kamikaze\MED_CH_GenericDrone_01",
                @"characters\kamikaze\TEX_CH_GenericDrone_01",
                rotation: new Vector3(0, 0, MathF.PI / 2),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"), 
                standardEffectInitialize: new Tuple<string, float>[] {
                    new Tuple<string, float>("LineThickness", 0.01f)},
                scale: new Vector3(6f)
                )));
            entity.Set(new DamageComponent(200f));
        }

        public static void SpawnMailBox(Vector2 position)
        {
            Entity entity = SpawnHelper.SpawnBasicEnemy(position);
            entity.Set(new NameComponent() { name = Constants.MAILBOX_NAME });

            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"characters\MED_CH_Mailbox_01",
                @"characters\mailbox_tex",
                rotation: new Vector3(Vector2.Zero, -MathF.PI / 2),
                scale: new Vector3(3f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/outline"),
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.05f) }
                )));
            entity.Set(new AIComponent(new MailboxOffline(entity)));

            entity.Set(new SmallGunComponent(
                damage: Constants.MAILBOX_DAMAGE,
                projectileSpeed: Constants.BULLET_SPEED,
                radiusRange: -1f,
                reloadTime: Constants.MAILBOX_RELOAD_TIME,
                projectileSkin: "EnemyProjectile_01",
                projectileAnimation: "EnemyProjectile_01",
                alliance: Alliance.Hostile));

        }

        public static void SpawnRandomEnemy(bool shooter, Vector2 seed, int range)
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

            if (shooter) SpawnHelper.SpawnShooter(position);
            else SpawnHelper.SpawnKamikaze(position);

        }

        public static void SpawnEnemyCamp(Vector2 position) 
        {
            int enemyCount = Constants.RANDOM.Next(5, 8);
            bool upgrade = Constants.RANDOM.NextDouble() <= HelperFunctions.PowerUpRate(); 
            if (upgrade) SpawnHelper.SpawnPowerUp(position);

            for (int i = 0; i < enemyCount; ++i)
            {
                bool shooter = Constants.RANDOM.Next(3) == 0;
                SpawnHelper.SpawnRandomEnemy(shooter, position, 50);
            }
        }

        public static void SpawnEnemyCamps()
        {
            Debug.WriteLine("START: Spawning Enemy Camps");
            int step = (int)Constants.TILE_SIZE * 10;

            for (float y = Constants.BOTTOM_BORDER; y < Constants.TOP_BORDER; y += step)
            {
                for (float x = Constants.LEFT_BORDER; x < Constants.RIGHT_BORDER; x += step)
                {
                    Vector2 curr = new Vector2(x, y);
                    if (curr.LengthSquared() <= (100 * 100)) continue;
                    bool gen =
                        Constants.RANDOM.NextDouble() < ProcGen.SpawnMap.GetSpawnRate(curr);
                    if (gen)
                    {
                        //Debug.WriteLine("New Hotspot at: " + curr);
                        SpawnHelper.SpawnEnemyCamp(curr);
                    }
                }
            }
            Debug.WriteLine("END: Spawning Enemy Camps");

        }

        public static Entity SpawnBullet(Vector3 position, Vector2 direction)
        {
            var entity = SpawnHelper.World.CreateEntity();

            SpawnHelper.AttachAABB(entity, position.ToVector2(), Constants.BULLET_SIZE, Constants.BULLET_SIZE, false);

            // Since it is a texture the rotation is in World Space (I guess)
            float dirRot = -Camera2D.WorldToPerspective(direction).ToRotation() + MathHelper.Pi;

            var projectileTransform = new Transform3DComponent(new Transform3D(position, rotation: new Vector3(0,0,dirRot)));
            entity.Set(projectileTransform);
            entity.Set(new WorldSpaceComponent());

            return entity;
        }

        public static StoryIntroEvent SpawnEvent()
        {
            var entity = SpawnHelper.World.CreateEntity();

            var _event = new StoryIntroEvent();
            _event.Initialize(SpawnHelper.World, entity);

            entity.Set(new EventComponent(_event));
            entity.Set(new NameComponent() { name = "intro_event" });
            return _event;
        }

        public static void SpawnBasicWall(Vector2 center, float height, float width)
        {
            var entity = SpawnHelper.World.CreateEntity();
            SpawnHelper.AttachAABB(entity, center, width, height, true);
            //Create entity and attach its components
            entity.Set(new Transform2DComponent(new Transform2D(center)));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new NameComponent() { name = Constants.WALL_NAME });
        }

        public static void SpawnRoadBlock(Vector2 position, int direction)
        {
            if (!SpawnHelper.IsCollisionFree(new AABB(position, 6, 6), true)) return;
            var entity = SpawnHelper.World.CreateEntity();
            
            entity.Set(new NameComponent() { name = Constants.ROADBLOCK_NAME});

            //TODO: refactor
            if (direction == 0)
                SpawnHelper.AttachAABB(entity, position, new Vector2(-2, -5), new Vector2(2, 5), true);
            else
                SpawnHelper.AttachAABB(entity, position, new Vector2(-5, -2), new Vector2(5, 2), true);

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

        public static void AttachAABB(Entity entity, Vector2 position, Vector2 lowerBound, Vector2 upperBound, bool solid)
        {
            AABB aabb = new AABB(lowerBound, upperBound);
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;

            SpawnHelper.quadtree.AddNode(element);
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, solid));
        }

        public static void AttachAABB(Entity entity, Vector2 position, float width, float height, bool solid)
        {
            Vector2 lowerBound = new Vector2(-width / 2, -height / 2);
            Vector2 upperBound = new Vector2(width / 2, height / 2);
            SpawnHelper.AttachAABB(entity, position, lowerBound, upperBound, solid);

        }

    }
}


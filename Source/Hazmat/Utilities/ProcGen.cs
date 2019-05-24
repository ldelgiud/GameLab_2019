using DefaultEcs;
using DefaultEcs.Resource;
using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision;

namespace Hazmat.Utilities
{
    class ProcGen
    {
        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            }
        }

        public static World World
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<World>();
            }
        }

        public static TileMap TileMap
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<TileMap>();
            }
        }

        public static SpawnMap SpawnMap
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<SpawnMap>();
            }
        }

        public static Street Street
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<Street>();
            }
        }

        public static void BuildBackground()
        {
            Debug.WriteLine("START: Background Generation");
            float x = Constants.LEFT_BORDER;
            float y = Constants.TOP_BORDER;
            float tile = Constants.TILE_SIZE;
            while (y >= Constants.BOTTOM_BORDER)
            {
                while (x <= Constants.RIGHT_BORDER)
                {
                    var position = new Vector2(x, y);

                    //Build Walls
                    if (x == Constants.BOTTOM_BORDER + 5* tile || 
                        x == Constants.TOP_BORDER - 5 * tile ||
                        y == Constants.LEFT_BORDER + 5 * tile ||
                        y == Constants.RIGHT_BORDER - 5* tile)
                    {
                        SpawnHelper.SpawnBasicWall(new Vector2(x + tile / 2, y + tile / 2), tile, tile);
                    }

                    //Add grass Background
                    ProcGen.TileMap.AddTile(
                        new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                        new TileModelInfo(@"static_sprites/SPT_EN_Tile_Grass_01"),
                        "grassTile"
                        );
                    
                    x += Constants.TILE_SIZE;
                }
                x = Constants.LEFT_BORDER;
                y -= Constants.TILE_SIZE;
            }
            Debug.WriteLine("END: Background Generation");
        }

        public static void BuildPowerPlant(PowerPlant plant)
        {
            Debug.WriteLine("START: Powerplant Generation");
            var entity = SpawnHelper.World.CreateEntity();
            entity.Set(new NameComponent() { name = Constants.POWERPLANT_NAME });

            //Generate random position
            double angle = Constants.RANDOM.NextDouble() * (Constants.MAX_RADIAN - Constants.MIN_RADIAN) + Constants.MIN_RADIAN;
            double x = Math.Round(Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle) / 10) * 10;
            //TODO: change this once camera work is done
            double y = Math.Round(Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle) / 10) * 10;
            Vector2 position = new Vector2((float)x, (float)y);
            plant.Position = position;

            //Bounding box stuff
            SpawnHelper.AttachAABB(entity, position, 50, 50, true);

            //Create entity and attach the components to it
            entity.Set(new Transform3DComponent(new Transform3D(new Vector3(position, 0))));
            entity.Set(new WorldSpaceComponent());
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\houses\MES_EN_Powerplant_01",
                @"buildings\houses\TEX_EN_Powerplant_01",
                scale: new Vector3(0.2f,0.2f,0.1f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                updateTimeEffect: true,
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.1f) }
            )));
            entity.Set(new PowerPlantComponent());
            entity.Set(new InteractableComponent());

            Debug.WriteLine("End: Powerplant Generation");

        }

        public static bool ToCloseTooStreet(Vector2 position)
        {
            bool streetFound = false;
            AABB testAABB = new AABB()
            {
                LowerBound = new Vector2(-7f, -7f),
                UpperBound = new Vector2(7f, 7f)
            };
            testAABB.LowerBound += position;
            testAABB.UpperBound += position;

            ProcGen.TileMap.quadtree.QueryAABB((Element<Entity> collidee) =>
            {
                if (collidee.Value.Has<NameComponent>())
                {
                    if (collidee.Value.Get<NameComponent>().name == Constants.STREET_TILE_NAME)
                    {
                        streetFound = true;
                        return false;
                    }
                }
                return true;
            }, ref testAABB);
                
            return streetFound;
        }

        public static void BuildExtras()
        {
            Debug.WriteLine("START: Objects Generation");

            float tile = Constants.TILE_SIZE;
            for (float y = Constants.BOTTOM_BORDER; y <= Constants.TOP_BORDER; y+= Constants.TILE_SIZE)
            {
                for (float x = Constants.LEFT_BORDER; x <= Constants.RIGHT_BORDER; x += Constants.TILE_SIZE)
                {
                    float randRradian = (float) (Constants.RANDOM.NextDouble() * Math.PI / 2);
                    Vector2 position = new Vector2(x, y);
                    Vector2 closestTile = Street.FindClosestTile(position);
                    float streetSqrdDist = (position- closestTile).LengthSquared();
                    
                    if (position.LengthSquared() <= 30*30) continue;
                    //Add random objects 
                    int rand = Constants.RANDOM.Next(100);
                    if (rand <= 10 && streetSqrdDist >= 10 * 10)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(
                                new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), 
                                scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_02"),
                            "dirtTile"
                            );
                    } else if (rand <= 20 && streetSqrdDist >= 10 * 10)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(
                                new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), 
                                scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_03"),
                            "dirtTile"
                            );
                    } else if (rand <= 30 && streetSqrdDist >= 10 * 10)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(
                                new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), 
                                scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_04"),
                            "dirtTile"
                            );
                    }
                    else if (rand <= 36 && streetSqrdDist >= 20 * 20)
                    {
                        SpawnHelper.SpawnRandomHouse(position,100);
                    }
                    else if (rand <= 41 && streetSqrdDist >= 30 * 30)
                    {
                        SpawnHelper.SpawnRock(position, Constants.RANDOM.Next(1, 5));
                    } 
                }
            }
            Debug.WriteLine("END: Objects Generation");

        }

        static void RemoveObjects(Vector2 position)
        {
            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-4.5f, -4.5f),
                UpperBound = new Vector2(4.5f, 4.5f)
            };

            Element<Entity> element = new Element<Entity>(aabb);
            element.Span.LowerBound += position;
            element.Span.UpperBound += position;
            List<Entity> entities = SpawnHelper.CollisionCheck(element.Span, true);

            foreach (Entity ent in entities)
            {
                ent.Delete();
            }
        }

        public static void PlaceStreetTile(
            Vector2 position, 
            int direction, 
            bool changeDir, 
            int parity, 
            bool walls, 
            bool turnedBefore,
            bool willTurnNext)
        {
            ProcGen.Street.AddTile(position);
            ProcGen.TileMap.RemoveTiles(new Transform2D(position));
            float radian = direction * MathF.PI / 2;
            //Add roadblock
            bool block = Constants.RANDOM.Next(8) == 1;
            if (block && !changeDir)
            {
                Vector2 roadblockOffset = new Vector2(0, 4f).Rotate(radian);
                if (Constants.RANDOM.Next(2) == 0) roadblockOffset *= -1;
                SpawnHelper.SpawnRoadBlock(position+roadblockOffset, direction);
            }
            //Add house
            bool house1 = Constants.RANDOM.Next(100) <= 60;
            bool house2 = Constants.RANDOM.Next(100) <= 60; 
            if (walls && !changeDir && !turnedBefore && !willTurnNext)
            {
                Vector2 houseOffset = new Vector2(0,25).Rotate(radian);
                if (house1) SpawnHelper.SpawnRandomHouse(position+houseOffset, (direction-1)%4);
                if (house2) SpawnHelper.SpawnRandomHouse(position-houseOffset, (direction+1)%4);
            }
            Vector3 rotation = new Vector3(Vector2.Zero, (1 - direction) * MathF.PI / 2);
            float step = Constants.TILE_SIZE / 4;

            if (changeDir)
            {
                if (direction == 0) rotation.Z += MathF.PI / 2; 
                Vector2 extraSideWalkOffset =
                    new Vector2(3 * Constants.TILE_SIZE / 4, -3 * Constants.TILE_SIZE / 4)
                    .Rotate(direction * MathF.PI);
                //Turn Left
                Vector2 extraSidewalkBarrierOffset1 =
                    new Vector2(Constants.TILE_SIZE, - 3 * Constants.TILE_SIZE / 4)
                    .Rotate(direction * MathF.PI);
                Vector2 extraSidewalkBarrierOffset2 =
                     new Vector2(3 * Constants.TILE_SIZE / 4, -Constants.TILE_SIZE)
                     .Rotate(direction * MathF.PI);
                SpawnHelper.SpawnCompleteSidewalk(position, direction, walls);
                SpawnHelper.SpawnCompleteSidewalk(position, 3-direction, walls);
                SpawnHelper.SpawnSideWalk(position + extraSideWalkOffset, 100);
                SpawnHelper.SpawnLamp(position + extraSideWalkOffset, MathF.PI / 4);

                if (walls )
                {
                    SpawnHelper.SpawnSmallSidewalkBarrier(position + extraSidewalkBarrierOffset1, 1);
                    SpawnHelper.SpawnSmallSidewalkBarrier(position + extraSidewalkBarrierOffset2, 0);
                }

                ProcGen.TileMap.AddTile(
                        new Transform3D(
                            new Vector3(position, Constants.LAYER_BACKGROUND),
                            rotation: rotation,
                            scale: new Vector3(5f)),
                        new TileModelInfo(
                            "static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                        Constants.STREET_TILE_NAME
                        );
            }
            else
            {
                if ((!willTurnNext && !turnedBefore) || !walls)
                {
                    SpawnHelper.SpawnCompleteSidewalk(position, (direction + 3) % 4, walls);
                    SpawnHelper.SpawnCompleteSidewalk(position, (direction + 1) % 4, walls);
                } else if (willTurnNext)
                {
                    if (direction == 0)
                    {
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 3) % 4, walls);
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 1) % 4, false);
                        Vector2 BarrierOffset = new Vector2(-Constants.TILE_SIZE / 4, Constants.TILE_SIZE);
                        SpawnHelper.SpawnSmallSidewalkBarrier(position + BarrierOffset, 0);
                    } else
                    {
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 1) % 4, walls);
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 3) % 4, false);
                        Vector2 BarrierOffset = new Vector2(Constants.TILE_SIZE, -Constants.TILE_SIZE / 4);
                        SpawnHelper.SpawnSmallSidewalkBarrier(position + BarrierOffset, 1);
                    }

                }
                else if (turnedBefore)
                {
                    if (direction == 0)
                    {
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 3) % 4, false);
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 1) % 4, walls);
                        Vector2 BarrierOffset = new Vector2(Constants.TILE_SIZE / 4, -Constants.TILE_SIZE);
                        SpawnHelper.SpawnSmallSidewalkBarrier(position + BarrierOffset, 0);
                    }
                    else
                    {
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 1) % 4, false);
                        SpawnHelper.SpawnCompleteSidewalk(position, (direction + 3) % 4, walls);
                        Vector2 BarrierOffset = new Vector2(-Constants.TILE_SIZE, Constants.TILE_SIZE / 4);
                        SpawnHelper.SpawnSmallSidewalkBarrier(position + BarrierOffset, 1);
                    }
                }
                if (parity==1)
                {
                    Vector2 LampOffset = new Vector2(Constants.TILE_SIZE / 4, 3 * Constants.TILE_SIZE / 4)
                        .Rotate(radian);
                    SpawnHelper.SpawnLamp(position + LampOffset, MathF.PI / 2 - radian);
                }
                else if (parity == 3)
                {
                    Vector2 LampOffset = new Vector2(Constants.TILE_SIZE / 4,- 3 * Constants.TILE_SIZE / 4)
                        .Rotate(radian);
                    SpawnHelper.SpawnLamp(position + LampOffset, MathF.PI / 2 - radian);
                }
                // Right
                ProcGen.TileMap.AddTile(
                    new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: rotation),
                    new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                    Constants.STREET_TILE_NAME
                    );
            }
            ProcGen.SpreadClothes(position);
        }

        private static void SpreadClothes(Vector2 position)
        {
            float radian = (float) Constants.RANDOM.NextDouble() * MathF.PI * 2;
            int counter = 0;
            Vector2 offset = new Vector2(2.5f, 0);
            float radianOffset = (MathF.PI + (float)Constants.RANDOM.NextDouble() * MathF.PI) / 4;
            while(Constants.RANDOM.Next(2)==0 && counter < 4)
            {
                counter++;
                offset = offset.Rotate(radianOffset);
                int clotheType = Constants.RANDOM.Next(1, 7);
                ProcGen.TileMap.AddTile(
                        new Transform3D(
                            new Vector3(position+offset, 0.1f),
                            rotation: new Vector3(Vector2.Zero, radian),
                            scale: new Vector3(1f)),
                        new TileModelInfo(
                            "static_sprites/SPT_EN_Clothes_0" + clotheType),
                        "clothes"
                        );
            }
            
        }

        public static void BuildStreet(PowerPlant plant)
        {
            Debug.WriteLine("START: Street Generation");

            Vector2 curr = Vector2.Zero;
            Vector3 scale = new Vector3(0.2f, 0.2f, 1);
            
            //Target position is diagonally previous tile of plants tile
            int x = (int)(plant.Position.X / Constants.TILE_SIZE) - 3;
            int y = (int)(plant.Position.Y / Constants.TILE_SIZE) - 3;

            Vector2 target = new Vector2(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE);
            //0 = right; 1 = top;
            int currentDir = 0;
            bool walls = false;
            int parity = 0;
            bool changeDir = false;
            bool prevChangeDir = changeDir;
            bool nextChangeDir;
            int nextLootStation = Constants.LOOT_STATIONS_DIST;

            while (curr.X < target.X && curr.Y < target.Y)
            {
                nextChangeDir = Constants.RANDOM.Next(7) == 1 && (!prevChangeDir) && (!changeDir);
                if(curr.X == nextLootStation || curr.Y == nextLootStation)
                {
                    nextLootStation += Constants.LOOT_STATIONS_DIST;
                    SpawnHelper.SpawnLootStation(curr,1-currentDir);
                }
                ProcGen.PlaceStreetTile(
                    curr, currentDir, changeDir, parity, walls, prevChangeDir, nextChangeDir);
                if ((changeDir && currentDir == 0) || (!changeDir && currentDir == 1))
                {
                    curr.Y += Constants.TILE_SIZE;
                } else
                {
                    curr.X += Constants.TILE_SIZE;
                }
                if (changeDir)
                {
                    currentDir = 1 - currentDir;
                    parity = 0;
                }
                else parity = (++parity)%4;
                if (walls)
                {
                    walls = Constants.RANDOM.Next(100) <= 80;
                } else walls = Constants.RANDOM.Next(100) > 80;
                if ((curr.X + Constants.TILE_SIZE) >= target.X || (curr.Y + Constants.TILE_SIZE) >= target.Y)
                {
                    walls = false;
                }
                prevChangeDir = changeDir;
                changeDir = nextChangeDir;
            }

            while (curr.X < target.X)
            {
                //Go right
                if (currentDir == 0)
                {
                    ProcGen.PlaceStreetTile(curr, 0, false, parity, walls,prevChangeDir,false);
                    prevChangeDir = false;
                }
                else
                {
                    ProcGen.PlaceStreetTile(curr, 1, true, parity, walls, prevChangeDir, false);
                    currentDir = 0;
                    prevChangeDir = true;
                } 

                curr.X += Constants.TILE_SIZE;
                parity = (++parity)%4;
                if (walls) walls = Constants.RANDOM.Next(100) <= 50;
                else walls = Constants.RANDOM.Next(100) > 50;
            }

            while (curr.Y < target.Y)
            {
                if (currentDir == 1)
                {
                    ProcGen.PlaceStreetTile(curr, 1, false, parity, walls,prevChangeDir, false);
                    prevChangeDir = false;
                }
                else
                {
                    ProcGen.PlaceStreetTile(curr, currentDir, true, parity, walls, prevChangeDir, false);
                    prevChangeDir = true;
                    currentDir = 1;
                }

                //Go to next Tile
                curr.Y += Constants.TILE_SIZE;
                parity = (++parity)%4;
                if (walls) walls = Constants.RANDOM.Next(100) <= 50;
                else walls = Constants.RANDOM.Next(100) > 50;
            }
        }

        public static void SetSpawnRates()
        {
            Debug.WriteLine("START: Set Spawning Rates");
            foreach (Vector2 tile in Street.positions)
            {
                for (int i = -5; i <= 5; i++)
                {
                    float x = tile.X + i * ProcGen.SpawnMap.tilesize;
                    Vector2 myTile =
                        new Vector2(x, tile.Y);
                    double divisor = Math.Pow(2, Math.Abs(i));

                    if (x > Constants.LEFT_BORDER && x < Constants.RIGHT_BORDER)
                    {
                        ProcGen.SpawnMap.SetSpawnRate(myTile, Constants.STREET_SPAWN_RATE/divisor);
                    }
                }
            }
            Debug.WriteLine("END: Set Spawning Rates");
        }
    }
}

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
            entity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                @"buildings\houses\MES_EN_Powerplant_01",
                @"buildings\houses\TEX_EN_Powerplant_01",
                scale: new Vector3(0.1f, 0.1f, 0.05f),
                standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/toon"),
                updateTimeEffect: true,
                standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", 0.1f) }
            )));
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, true));
            entity.Set(new PowerPlantComponent());
            entity.Set(new InteractableComponent());

            SpawnHelper.quadtree.AddNode(element);

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
                    
                    Vector2 position = new Vector2(x, y);
                    Vector2 closestTile = Street.FindClosestTile(position);
                    float streetSqrdDist = (position- closestTile).LengthSquared();
                    
                    if (position.LengthSquared() <= 30*30 || streetSqrdDist <= 20*20) continue;
                    //Add random objects 
                    int rand = Constants.RANDOM.Next(100);
                    if (rand <= 15)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_02"),
                            "dirtTile"
                            );
                    }
                    else if (rand <= 19)
                    {
                        SpawnHelper.SpawnRandomHouse(position);
                    }
                    else if (rand <= 21)
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

        public static void PlaceStreetTile(Vector2 position, int direction, bool changeDir, int parity)
        {
            ProcGen.Street.AddTile(position);
            ProcGen.TileMap.RemoveTiles(new Transform2D(position));

            //Add boulder
            bool block = Constants.RANDOM.Next(8) == 1;
            if (block && !changeDir) SpawnHelper.SpawnRoadBlock(position, direction);

            Vector3 rotation = Vector3.Zero;

            Vector2 lowerSidewalkPos1;
            Vector2 lowerSidewalkPos2;
            Vector2 upperSidewalkPos1;
            Vector2 upperSidewalkPos2;
            float step = Constants.TILE_SIZE / 4;
            int dir1, dir2;

            if (changeDir)
            {
                Vector2 extraSideWalk;
                //Turn Left
                if (direction == 0)
                {
                    //Down
                    lowerSidewalkPos1 = new Vector2(position.X - step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    lowerSidewalkPos2 = new Vector2(position.X + step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    dir1 = 2;
                    //Right
                    upperSidewalkPos1 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    upperSidewalkPos2 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    dir2 = 1;
                    //Down & right
                    extraSideWalk = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - 3 * Constants.TILE_SIZE / 4);
                    // Left turn
                    rotation = new Vector3(0, 0, MathF.PI);
                } else
                {
                    //Up
                    upperSidewalkPos1 = new Vector2(position.X - step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    upperSidewalkPos2 = new Vector2(position.X + step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    dir1 = 3;
                    //Left
                    lowerSidewalkPos1 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    lowerSidewalkPos2 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    dir2 = 0;
                    //Up & Left
                    extraSideWalk = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + 3* Constants.TILE_SIZE / 4);

                }
                SpawnHelper.SpawnLamp(extraSideWalk, -MathF.PI/4);
                SpawnHelper.SpawnSidewalkWithWall(upperSidewalkPos1, dir1);
                SpawnHelper.SpawnSidewalkWithWall(upperSidewalkPos2, dir1);
                SpawnHelper.SpawnSidewalkWithWall(lowerSidewalkPos1, dir2);
                SpawnHelper.SpawnSidewalkWithWall(lowerSidewalkPos2, dir2);
                SpawnHelper.SpawnSideWalk(extraSideWalk);

                ProcGen.TileMap.AddTile(
                        new Transform3D(
                            new Vector3(position, Constants.LAYER_BACKGROUND),
                            rotation: rotation,
                            scale: new Vector3(5f)),
                        new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                        Constants.STREET_TILE_NAME
                        );
            }
            else
            {
                
                //Go right
                if (direction == 0)
                {
                    rotation = new Vector3(0, 0, -MathF.PI / 2);
                    //Down
                    lowerSidewalkPos1 = new Vector2(position.X - step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    lowerSidewalkPos2 = new Vector2(position.X + step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    dir1 = 1;
                    //Up
                    upperSidewalkPos1 = new Vector2(position.X - step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    upperSidewalkPos2 = new Vector2(position.X + step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    dir2 = 3;
                } else
                {
                    //Left
                    lowerSidewalkPos1 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    lowerSidewalkPos2 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    dir1 = 0;
                    //Right
                    upperSidewalkPos1 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    upperSidewalkPos2 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    dir2 = 2;

                }

                SpawnHelper.SpawnSidewalkWithWall(upperSidewalkPos1, dir2);
                SpawnHelper.SpawnSidewalkWithWall(upperSidewalkPos2, dir2);
                SpawnHelper.SpawnSidewalkWithWall(lowerSidewalkPos1, dir1);
                SpawnHelper.SpawnSidewalkWithWall(lowerSidewalkPos2, dir1);
                    
                if (parity==3) SpawnHelper.SpawnLamp(lowerSidewalkPos1, rotation.Z);
                else if (parity == 1) SpawnHelper.SpawnLamp(upperSidewalkPos1, rotation.Z);
                // Right
                ProcGen.TileMap.AddTile(
                    new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: rotation),
                    new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                    Constants.STREET_TILE_NAME
                    );
            }

            

        }

        public static void BuildStreet(PowerPlant plant)
        {
            Debug.WriteLine("START: Street Generation");

            Vector2 curr = Vector2.Zero;
            Vector3 scale = new Vector3(0.2f, 0.2f, 1);
            
            //Target position is diagonally previous tile of plants tile
            int x = (int)(plant.Position.X / Constants.TILE_SIZE);
            int y = (int)(plant.Position.Y / Constants.TILE_SIZE);

            Vector2 target = new Vector2(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE);
            //0 = right; 1 = top;
            int currentDir = Constants.RANDOM.Next(2);
            int parity = 0;
            while (curr.X < target.X && curr.Y < target.Y)
            {
                bool changeDir = Constants.RANDOM.Next(8) == 1;
                ProcGen.PlaceStreetTile(curr, currentDir, changeDir, parity);
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
            }

            while (curr.X < target.X)
            {
                //Go right
                if (currentDir == 0) ProcGen.PlaceStreetTile(curr, currentDir, false, parity);
                else ProcGen.PlaceStreetTile(curr, currentDir, true, parity);

                curr.X += Constants.TILE_SIZE;
                currentDir = 0;
                parity = (++parity)%4;

            }

            while (curr.Y < target.Y)
            {
                if (currentDir == 0) ProcGen.PlaceStreetTile(curr, currentDir, true, parity);
                else ProcGen.PlaceStreetTile(curr, 1, false, parity);
                
                //Go to next Tile
                currentDir = 1;
                curr.Y += Constants.TILE_SIZE;
                parity = (++parity)%4;
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

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

        public static void BuildBackground()
        {
            Debug.WriteLine("Background Generation");

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
            Debug.WriteLine("Objects Generation");
            float tile = Constants.TILE_SIZE;
            for (float y = Constants.BOTTOM_BORDER; y <= Constants.TOP_BORDER; y+= Constants.TILE_SIZE)
            {
                for (float x = Constants.LEFT_BORDER; x <= Constants.RIGHT_BORDER; x += Constants.TILE_SIZE)
                {
                    
                    Vector2 position = new Vector2(x, y);
                    if (ProcGen.ToCloseTooStreet(position)) continue;
                    if (position.Length() <= 20) continue;
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

            ProcGen.TileMap.RemoveTiles(new Transform2D(position));

            //Add boulder
            bool block = Constants.RANDOM.Next(8) == 1;
            if (block && !changeDir) SpawnHelper.SpawnRoadBlock(position, direction);

            ProcGen.SpawnMap.map
                    [(int)(position.Y / (Constants.TILE_SIZE * 10))]
                    [(int)(position.X / (Constants.TILE_SIZE * 10))] = Constants.STREET_SPAWN_RATE;
            Vector3 rotation = Vector3.Zero;

            Vector2 lowerSidewalkPos1;
            Vector2 lowerSidewalkPos2;
            Vector2 upperSidewalkPos1;
            Vector2 upperSidewalkPos2;
            float step = Constants.TILE_SIZE / 4;

            if (changeDir)
            {
                Vector2 extraSideWalk;
                //Turn Left
                if (direction == 0)
                {
                    //Down
                    lowerSidewalkPos1 = new Vector2(position.X - step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    lowerSidewalkPos2 = new Vector2(position.X + step, position.Y - 3 * Constants.TILE_SIZE / 4);
                    
                    //Right
                    upperSidewalkPos1 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    upperSidewalkPos2 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    //Down & right
                    extraSideWalk = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - 3 * Constants.TILE_SIZE / 4);
                    // Left turn
                    rotation = new Vector3(0, 0, MathF.PI);
                } else
                {
                    //Up
                    upperSidewalkPos1 = new Vector2(position.X - step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    upperSidewalkPos2 = new Vector2(position.X + step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    //Left
                    lowerSidewalkPos1 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    lowerSidewalkPos2 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    //Up & Left
                    extraSideWalk = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + 3* Constants.TILE_SIZE / 4);

                }
                SpawnHelper.SpawnLamp(extraSideWalk, -MathF.PI/4);
                SpawnHelper.SpawnSidewalk(upperSidewalkPos1, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(upperSidewalkPos2, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(lowerSidewalkPos1, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(lowerSidewalkPos2, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(extraSideWalk, Vector3.Zero);
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
                    //Up
                    upperSidewalkPos1 = new Vector2(position.X - step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    upperSidewalkPos2 = new Vector2(position.X + step, position.Y + 3 * Constants.TILE_SIZE / 4);
                    
                } else
                {
                    //Left
                    lowerSidewalkPos1 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    lowerSidewalkPos2 = new Vector2(position.X - 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    //Right
                    upperSidewalkPos1 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y - step);
                    upperSidewalkPos2 = new Vector2(position.X + 3 * Constants.TILE_SIZE / 4, position.Y + step);
                    
                }
                
                SpawnHelper.SpawnSidewalk(upperSidewalkPos1, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(upperSidewalkPos2, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(lowerSidewalkPos1, Vector3.Zero);
                SpawnHelper.SpawnSidewalk(lowerSidewalkPos2, Vector3.Zero);
                    
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
            Debug.WriteLine("Street Generation");

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
            Debug.WriteLine("Setting Spawning Rates");
            int Y = (int) (Constants.TOP_BORDER / (Constants.TILE_SIZE * 10)) - 1;
            int X = (int) (Constants.RIGHT_BORDER / (Constants.TILE_SIZE * 10)) - 1;
            int x = 0;
            int y = 0;
            
            while (x < X && y < Y)
            {
                //Debug.WriteLine("Position: " + new Vector2(x,y));
                for (int i = 0; i < 5; i++)
                {
                    double divisor = Math.Pow(2.0, i);
                    if (x+i < X) ProcGen.SpawnMap.map[y][x+i] = 
                            Math.Max(ProcGen.SpawnMap.map[y][x], Constants.STREET_SPAWN_RATE / divisor);
                    if (x-i > 0) ProcGen.SpawnMap.map[y][x - i] =
                            Math.Max(ProcGen.SpawnMap.map[y][x], Constants.STREET_SPAWN_RATE / divisor);
                }

                if ((ProcGen.SpawnMap.map[y + 1][x] == Constants.STREET_SPAWN_RATE)) y++;
                else if (ProcGen.SpawnMap.map[y][x + 1] == Constants.STREET_SPAWN_RATE) x++;
                else
                {
                    //Debug.WriteLine("No More street at: (" + x + "," + y + ")");
                    break;
                }
            }
        }

        public static void SpawnHotspots()
        {
            Debug.WriteLine("Spawning Enemy Camps");
            int step = (int)Constants.TILE_SIZE * 10;
            for (int y = 100; y < Constants.TOP_BORDER; y += step)
            {
                for (int x = 100; x < Constants.RIGHT_BORDER; x += step)
                {
                    Vector2 curr = new Vector2(x, y);
                    bool gen =
                        Constants.RANDOM.NextDouble() < ProcGen.SpawnMap.map[y/step][x/step];
                    if (gen)
                    {
                        Debug.WriteLine("New Hotspot at: " + curr);
                        SpawnHelper.SpawnEnemyCamp(curr);
                    }
                }
            }
        }
    }
}

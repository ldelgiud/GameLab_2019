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

        public static void BuildExtras()
        {
            Debug.WriteLine("Objects Generation");

            float x = Constants.LEFT_BORDER;
            float y = Constants.BOTTOM_BORDER;
            float tile = Constants.TILE_SIZE;
            while (y <= Constants.TOP_BORDER)
            {
                while (x <= Constants.RIGHT_BORDER)
                {
                    var position = new Vector2(x, y);
                    bool streetFound = false;

                    AABB testAABB = new AABB()
                    {
                        LowerBound = new Vector2(-1f, -1f),
                        UpperBound = new Vector2(1f, 1f)
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
                            }
                        }
                        return true;
                    }, ref testAABB);

                    if (streetFound)
                    {
                        x += Constants.TILE_SIZE;
                        continue;
                    }
                    
                    //Add random objects 
                    int rand = Constants.RANDOM.Next(100);
                    if (rand <= 25)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_02"),
                            "dirtTile"
                            );
                    }
                    else if (rand <= 30)
                    {
                        SpawnHelper.SpawnRandomHouse(position);
                    }

                    x += Constants.TILE_SIZE;
                }
                x = Constants.LEFT_BORDER;
                y += Constants.TILE_SIZE;
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
            List<Entity> entities = SpawnHelper.CollisionCheck(element.Span);

            foreach (Entity ent in entities)
            {
                ent.Delete();
            }
        }
    
        public static void BuildStreet(PowerPlant plant)
        {
            Debug.WriteLine("Street Generation");
            Vector2 curr = new Vector2(0);
            Vector3 scale = new Vector3(0.2f, 0.2f, 1);
            //Target position is diagonally previous tile of plants tile
            int x = (int)(plant.Position.X / Constants.TILE_SIZE);
            int y = (int)(plant.Position.Y / Constants.TILE_SIZE);

            Vector2 target = new Vector2(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE);
            //0 means right, 1 means top;
            int currentDir = Constants.RANDOM.Next(2);
            while (curr.X < target.X && curr.Y < target.Y)
            {
                ProcGen.SpawnMap.map
                    [(int) (curr.Y / (Constants.TILE_SIZE*10))]
                    [(int) (curr.X / (Constants.TILE_SIZE*10))] = Constants.STREET_SPAWN_RATE;
                //dir decides if we change the direction or if we keep going the current direction
                bool changeDir = Constants.RANDOM.Next(8) == 1;
                if (changeDir)
                {
                    if (currentDir == 0)
                    {
                        // Left turn
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                            Constants.STREET_TILE_NAME
                            );
                        curr.Y += Constants.TILE_SIZE;
                    }
                    else
                    {
                        // Right turn
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                            Constants.STREET_TILE_NAME
                            );
                        curr.X += Constants.TILE_SIZE;
                    }
                    currentDir = 1 - currentDir;
                }
                else
                {
                    if (currentDir == 0)
                    {
                        // Right
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, -MathF.PI / 2)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                            Constants.STREET_TILE_NAME
                            );
                        curr.X += Constants.TILE_SIZE;
                    }
                    else
                    {
                        // Up
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                            Constants.STREET_TILE_NAME
                            );
                        curr.Y += Constants.TILE_SIZE;
                    }
                }


                //Add boulder
                bool block = Constants.RANDOM.Next(8) == 1;
                if (block) SpawnHelper.SpawnRoadBlock(curr);
                
            }

            while (curr.X < target.X)
            {

                ProcGen.SpawnMap.map
                    [(int)(curr.Y / (Constants.TILE_SIZE * 10))]
                    [(int)(curr.X / (Constants.TILE_SIZE * 10))] = Constants.STREET_SPAWN_RATE;

                if (currentDir == 0)
                {
                    // Right
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI / 2)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                            Constants.STREET_TILE_NAME
                            );
                }
                else
                {
                    // Right Turn
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                            Constants.STREET_TILE_NAME
                            );
                }
                
               
                curr.X += Constants.TILE_SIZE;
                currentDir = 0;

                //Add boulder
                bool block = Constants.RANDOM.Next(8) == 1;
                if (block) SpawnHelper.SpawnRoadBlock(curr);

            }

            while (curr.Y < target.Y)
            {

                ProcGen.SpawnMap.map
                    [(int)(curr.Y / (Constants.TILE_SIZE * 10))]
                    [(int)(curr.X / (Constants.TILE_SIZE * 10))] = Constants.STREET_SPAWN_RATE;

                if (currentDir == 0)
                {
                    // Left turn
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01"),
                            Constants.STREET_TILE_NAME
                            );
                }
                else
                {
                    // Up
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01"),
                            Constants.STREET_TILE_NAME
                            );
                }
                
                //Go to next Tile
                currentDir = 1;
                curr.Y += Constants.TILE_SIZE;

                //Add boulder
                bool block = Constants.RANDOM.Next(8) == 1;
                if (block) SpawnHelper.SpawnRoadBlock(curr);


            }
        }

        public static void SetSpawnRates()
        {
            int Y = (int) (Constants.TOP_BORDER / (Constants.TILE_SIZE * 10)) - 1;
            int X = (int) (Constants.RIGHT_BORDER / (Constants.TILE_SIZE * 10)) - 1;
            int x = 0;
            int y = 0;
            Debug.WriteLine("X = " + X);
            Debug.WriteLine("Y = " + Y);

            while (x < X && y < Y)
            {
                Debug.WriteLine("Position: " + new Vector2(x,y));
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
                    Debug.WriteLine("No More street at: (" + x + "," + y + ")");
                    break;
                }
            }

            Debug.Write(ProcGen.SpawnMap.map);
        }

        public static void SpawnHotspots()
        {
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

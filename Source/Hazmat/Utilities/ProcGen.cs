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

namespace Hazmat.Utilities
{
    class ProcGen
    {

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

        public static void BuildWalls()
        {
            //Build Walls around the map
            float vertical = Constants.TOP_BORDER - Constants.BOTTOM_BORDER;
            float horizontal = Constants.RIGHT_BORDER - Constants.LEFT_BORDER;
            Vector2 leftWallPos = new Vector2(Constants.LEFT_BORDER, vertical / 2);
            Vector2 rightWallPos = new Vector2(Constants.RIGHT_BORDER, vertical / 2);
            Vector2 bottomWallPos = new Vector2(Constants.BOTTOM_BORDER, horizontal / 2);
            Vector2 topWallPos = new Vector2(Constants.TOP_BORDER, horizontal / 2);
            SpawnHelper.SpawnBasicWall(leftWallPos, vertical, 5f);
            SpawnHelper.SpawnBasicWall(rightWallPos, vertical, 5f);
            SpawnHelper.SpawnBasicWall(bottomWallPos, 5f, horizontal);
            SpawnHelper.SpawnBasicWall(topWallPos, 5f, horizontal);


        }
        public static void BuildBackground()
        {   
            float x = Constants.LEFT_BORDER;
            float y = Constants.TOP_BORDER;
            while (y >= Constants.BOTTOM_BORDER)
            {
                while (x <= Constants.RIGHT_BORDER)
                {
                    var position = new Vector2(x, y);

                    ProcGen.TileMap.AddTile(
                        new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                        new TileModelInfo(@"static_sprites/SPT_EN_Tile_Grass_01")
                        );

                    if (Constants.RANDOM.Next(100) <= 30)
                    {
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(position, Constants.LAYER_BACKGROUND_DETAIL), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_Grass_02")
                            );
                    }

                    x += Constants.TILE_SIZE;
                }
                x = Constants.LEFT_BORDER;
                y -= Constants.TILE_SIZE;
            }
        }

        public static void BuildStreet(PowerPlant plant)
        {
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
                //dir decides if we change the direction or if we keep going the current direction
                bool changeDir = Constants.RANDOM.Next(3) == 1;
                if (changeDir)
                {
                    if (currentDir == 0)
                    {
                        // Left turn
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01")
                            );
                        curr.Y += Constants.TILE_SIZE;
                    }
                    else
                    {
                        // Right turn
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01")
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
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01")
                            );
                        curr.X += Constants.TILE_SIZE;
                    }
                    else
                    {
                        // Up
                        ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                        ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01")
                            );
                        curr.Y += Constants.TILE_SIZE;
                    }
                }
            }

            while (curr.X <= target.X)
            {
                if (currentDir == 0)
                {
                    // Right
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI / 2)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01")
                            );
                }
                else
                {
                    // Right Turn
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01")
                            );
                }
                curr.X += Constants.TILE_SIZE;
                currentDir = 0;
            }

            while (curr.Y <= target.Y)
            {
                if (currentDir == 0)
                {
                    // Left turn
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f), rotation: new Vector3(0, 0, MathF.PI)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreetCorner_01")
                            );
                }
                else
                {
                    // Up
                    ProcGen.TileMap.RemoveTiles(new Transform2D(curr));
                    ProcGen.TileMap.AddTile(
                            new Transform3D(new Vector3(curr, Constants.LAYER_BACKGROUND), scale: new Vector3(5f)),
                            new TileModelInfo("static_sprites/SPT_EN_Tile_MainStreet_01")
                            );
                }
                currentDir = 1;
                curr.Y += Constants.TILE_SIZE;
            }
        }

        public static void SpawnHotspots()
        {
            for (float y = Constants.BOTTOM_BORDER; y < Constants.TOP_BORDER; y += Constants.TILE_SIZE * 10)
            {
                for (float x = Constants.LEFT_BORDER; x < Constants.RIGHT_BORDER; x += Constants.TILE_SIZE * 10)
                {
                    Vector2 curr = new Vector2(x, y);
                    bool gen =
                        Constants.RANDOM.NextDouble() < HelperFunctions.SpawnRate(curr);
                    if (gen)
                    {
                        SpawnHelper.SpawnEnemyCamp(curr);
                    }
                }
            }
        }
    }
}

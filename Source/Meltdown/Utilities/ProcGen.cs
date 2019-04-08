using DefaultEcs;
using DefaultEcs.Resource;
using Meltdown.Components;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Utilities
{
    class ProcGen
    {

        public static World World
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<World>();
            }
        }

        public static void BuildBackground()
        {
            float x = Constants.LEFT_BORDER;
            float y = Constants.TOP_BORDER;

            while (y >= Constants.BOTTOM_BORDER)
            {
                while (x <= Constants.RIGHT_BORDER)
                {

                    Vector2 position = new Vector2(x, y);

                    var entity = ProcGen.World.CreateEntity();
                    entity.Set(new Transform2DComponent(new Transform2D(position)));
                    entity.Set(new WorldSpaceComponent());
                    //entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/forest PLACEHOLDER", 100, 100)));
                    entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo("static_sprites/SPT_EN_Tile_Grass_01", width: 172.41f, height: 100)));
                    entity.Set(new NameComponent() { name = "background" });
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
            int x = (int) (plant.Position.X / Constants.TILE_SIZE);
            int y = (int) (plant.Position.Y / Constants.TILE_SIZE) ;

            Vector2 target = new Vector2(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE);
            //0 means right, 1 means top;
            int currentDir = Constants.RANDOM.Next(2);
            while (curr.X < target.X && curr.Y < target.Y)
            {

                var entity = ProcGen.World.CreateEntity();
                entity.Set(new Transform2DComponent(new Transform2D(curr)));
                entity.Set(new WorldSpaceComponent());
                entity.Set(new NameComponent() { name = "streetsign" });
                //dir decides if we change the direction or if we keep going the current direction
                bool changeDir = Constants.RANDOM.Next(3) == 1;
                if (changeDir)
                {
                    if (currentDir == 0)
                    {
                        entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/left turn", 10, 10)));
                        curr.Y += Constants.TILE_SIZE;
                    } else
                    {
                        entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/right turn", 10, 10)));
                        curr.X += Constants.TILE_SIZE;
                    }
                    currentDir = 1 - currentDir;
                }
                else
                {
                    if (currentDir == 0)
                    {
                        entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/right", 10, 10)));
                        curr.X += Constants.TILE_SIZE;
                    } else
                    {
                        entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/top", 10, 10)));
                        curr.Y += Constants.TILE_SIZE;
                    }
                }
            }

            while (curr.X <= target.X)
            {
                var entity = ProcGen.World.CreateEntity();
                entity.Set(new Transform2DComponent(new Transform2D(curr)));
                entity.Set(new WorldSpaceComponent());
                if (currentDir == 0)
                {
                    entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/right", 10, 10)));
                } else
                {
                    entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/right turn", 10, 10)));
                }
                entity.Set(new NameComponent() { name = "street" });
                curr.X += Constants.TILE_SIZE;
                currentDir = 0;
            }

            while (curr.Y <= target.Y)
            {
                var entity = ProcGen.World.CreateEntity();
                entity.Set(new Transform2DComponent(new Transform2D(curr)));
                entity.Set(new WorldSpaceComponent());
                if (currentDir == 0)
                {
                    entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/left turn", 10, 10)));
                }
                else
                {
                    entity.Set(new ManagedResource<Texture2DInfo, Texture2DAlias>(new Texture2DInfo(@"tiles/top", 10, 10)));
                }
                entity.Set(new NameComponent() { name = "street" });
                currentDir = 1;
                curr.Y += Constants.TILE_SIZE;
            }
        }

        public static void SpawnHotspots()
        {
            for (float y = Constants.BOTTOM_BORDER; y < Constants.TOP_BORDER; y += Constants.TILE_SIZE)
            {
                for (float x = Constants.LEFT_BORDER; x < Constants.RIGHT_BORDER; x += Constants.TILE_SIZE) 
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

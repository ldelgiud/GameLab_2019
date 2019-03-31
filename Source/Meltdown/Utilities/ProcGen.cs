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
                    Vector3 scale = new Vector3(
                        1.15f, 
                        0.85f,
                        1);
                    var entity = ProcGen.World.CreateEntity();
                    entity.Set(new WorldTransformComponent(
                        new Transform(
                            position : position.ToVector3(),
                            scale : scale)));
                    entity.Set(new ManagedResource<string, Texture2D>(@"tiles/forest PLACEHOLDER"));
                    entity.Set(new BoundingBoxComponent(Constants.TILE_SIZE, Constants.TILE_SIZE, 0));
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
            int x = ((int) (plant.Position.X / Constants.TILE_SIZE));
            int y = ((int) (plant.Position.Y / Constants.TILE_SIZE)) ;

            Vector2 target = new Vector2(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE);
            //0 means right, 1 means top;
            int currentDir = Constants.RANDOM.Next(2);
            while (curr.X < target.X && curr.Y < target.Y)
            {

                var entity = ProcGen.World.CreateEntity();
                entity.Set(new WorldTransformComponent(
                    new Transform(
                        position: curr.ToVector3(),
                        scale: scale)));
                entity.Set(new BoundingBoxComponent(15, 15, 0));
                //dir decides if we change the direction or if we keep going the current direction
                bool changeDir = Constants.RANDOM.Next(3) == 1;
                if (changeDir)
                {
                    if (currentDir == 0)
                    {
                        entity.Set(new ManagedResource<string, Texture2D>(@"tiles/left turn"));
                        curr.Y += Constants.TILE_SIZE;
                    } else
                    {
                        entity.Set(new ManagedResource<string, Texture2D>(@"tiles/right turn"));
                        curr.X += Constants.TILE_SIZE;
                    }
                    currentDir = 1 - currentDir;

                }
                else
                {
                    if (currentDir == 0)
                    {
                        entity.Set(new ManagedResource<string, Texture2D>(@"tiles/right"));
                        curr.X += Constants.TILE_SIZE;
                    } else
                    {
                        entity.Set(new ManagedResource<string, Texture2D>(@"tiles/top"));
                        curr.Y += Constants.TILE_SIZE;
                    }

                }
                 
            }

            while (curr.X <= target.X)
            {
                var entity = ProcGen.World.CreateEntity();
                entity.Set(new WorldTransformComponent(
                    new Transform(
                        position: curr.ToVector3(),
                        scale: scale)));
                entity.Set(new BoundingBoxComponent(15, 15, 0));
                if (currentDir == 0)
                {
                    entity.Set(new ManagedResource<string, Texture2D>(@"tiles/right"));
                } else
                {
                    entity.Set(new ManagedResource<string, Texture2D>(@"tiles/right turn"));
                }
                curr.X += Constants.TILE_SIZE;
                currentDir = 0;

            }

            while (curr.Y <= target.Y)
            {
                var entity = ProcGen.World.CreateEntity();
                entity.Set(new WorldTransformComponent(
                    new Transform(
                        position: curr.ToVector3(),
                        scale: scale)));
                entity.Set(new BoundingBoxComponent(15, 15, 0));
                if (currentDir == 0)
                {
                    entity.Set(new ManagedResource<string, Texture2D>(@"tiles/left turn"));
                }
                else
                {
                    entity.Set(new ManagedResource<string, Texture2D>(@"tiles/top"));
                }
                currentDir = 1;
                curr.Y += Constants.TILE_SIZE;
            }


        }


    }
}

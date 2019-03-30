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
                Debug.WriteLine("y = "+ y);
                while (x <= Constants.RIGHT_BORDER)
                {
                    Debug.WriteLine("x = " +x);

                    Vector2 position = new Vector2(x, y);
                    Vector3 scale = new Vector3(
                        Constants.TILE_SIZE / 100,
                        Constants.TILE_SIZE/ 100,
                        1);
                    var entity = SpawnHelper.World.CreateEntity();
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

        public static void BuildStreet(SpriteBatch spriteBatch)
        {

        }


    }
}

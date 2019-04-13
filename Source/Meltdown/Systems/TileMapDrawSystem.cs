using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Graphics;
using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.Systems
{
    class TileMapDrawSystem : ISystem<Time>
    {
        SpriteBatch spriteBatch;

        Camera2D camera;
        TileMap tileMap;
        

        public bool IsEnabled { get; set; } = true;

        public TileMapDrawSystem(GraphicsDevice graphicsDevice, Camera2D camera, TileMap tileMap)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.camera = camera;
            this.tileMap = tileMap;
        }

        public void Update(Time state)
        {
            var cameraPosition = this.camera.Transform.Translation;
            var x = Convert.ToInt32(cameraPosition.X);
            var y = Convert.ToInt32(cameraPosition.Y);
            var width = Convert.ToInt32(this.camera.ViewportWidth * 3);
            var height = Convert.ToInt32(this.camera.ViewportHeight * 3);

            this.spriteBatch.Begin();
            for (int i = x - width / 2; i < x + width / 2; ++i)
            {
                for (int j = y - height / 2; j < y + height / 2; ++j)
                {

                    if (this.tileMap.tiles.TryGetValue((i, j), out Tile tile))
                    {
                        ref Transform2DComponent transform = ref tile.transformComponent;
                        ref Texture2DComponent texture = ref tile.textureComponent;

                        var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform.value, texture.info);

                        var bounds = texture.info.bounds ?? texture.value.Bounds;
                        var origin = bounds.Size.ToVector2() / 2;

                        this.spriteBatch.Draw(
                            sourceRectangle: bounds,
                            texture: texture.value,
                            position: position,
                            rotation: rotation,
                            scale: scale,
                            origin: origin
                            );
                    }
                }
            }
            this.spriteBatch.End();
        }


        public void Dispose()
        {

        }
    }
}

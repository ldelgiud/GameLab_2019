using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.Systems
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

            this.spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

            var aabb = new AABB(
                new Vector2(cameraPosition.X - this.camera.ViewportWidth * 1.5f, cameraPosition.Y - this.camera.ViewportHeight * 1.5f),
                new Vector2(cameraPosition.X + this.camera.ViewportWidth * 1.5f, cameraPosition.Y + this.camera.ViewportHeight * 1.5f)
                );

            this.tileMap.quadtree.QueryAABB((element) =>
            {
                var entity = element.Value;
                ref var transform = ref entity.Get<Transform2DComponent>();
                ref var texture = ref entity.Get<Texture2DComponent>();

                var (position, rotation, scale) = this.camera.ToScreenCoordinates(transform.value, texture.info);

                var bounds = texture.info.bounds ?? texture.value.Bounds;
                var origin = bounds.Size.ToVector2() / 2;

                //Debug.WriteLine(texture.info.layer);
                this.spriteBatch.Draw(
                    sourceRectangle: bounds,
                    texture: texture.value,
                    position: position,
                    rotation: rotation,
                    scale: scale,
                    origin: origin,
                    layerDepth: texture.info.layer
                    );

                return true;
            }, ref aabb);

            this.spriteBatch.End();
        }

        public void Dispose()
        {

        }
    }
}

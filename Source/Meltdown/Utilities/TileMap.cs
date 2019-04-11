using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Meltdown.ResourceManagers;
using Meltdown.Components;
using Meltdown.Graphics;

namespace Meltdown.Utilities
{
    struct Tile
    {
        public Transform2DComponent transformComponent;
        public Texture2DComponent textureComponent;

        public Tile(Transform2DComponent transformComponent, Texture2DComponent textureComponent)
        {
            this.transformComponent = transformComponent;
            this.textureComponent = textureComponent;
        }
    }

    class TileMap
    {
        AtlasTextureResourceManager resourceManager;

        public Dictionary<(int, int), Tile> tiles = new Dictionary<(int, int), Tile>();


        public TileMap(AtlasTextureResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }


        public void AddTile(Transform2D transform, Texture2DInfo info)
        {
            var translation = transform.Translation;
            this.tiles[(Convert.ToInt32(translation.X), Convert.ToInt32(translation.Y))] =
                new Tile(new Transform2DComponent(transform), this.resourceManager.LoadDirect(info));
        }
    }
}

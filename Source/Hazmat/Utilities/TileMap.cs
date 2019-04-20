using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.ResourceManagers;
using Hazmat.Components;
using Hazmat.Graphics;

namespace Hazmat.Utilities
{
    class TileMap
    {
        World world = new World();
        AtlasTextureResourceManager resourceManager;

        public QuadTree<Entity> quadtree = new QuadTree<Entity>(new AABB(Constants.BOTTOM_LEFT_CORNER, Constants.TOP_RIGHT_CORNER), 10, 7);

        public TileMap(GraphicsDevice graphicsDevice, string atlasPath)
        {
            this.resourceManager = new AtlasTextureResourceManager(graphicsDevice, atlasPath);
            this.resourceManager.Manage(this.world);
        }


        public void AddTile(Transform2D transform, Texture2DInfo info)
        {
            var entity = this.world.CreateEntity();
            entity.Set(new Transform2DComponent(transform));
            entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(info));

            var element = new Element<Entity>(new AABB(transform.Translation, Constants.TILE_SIZE, Constants.TILE_SIZE))
            {
                Value = entity
            };
            this.quadtree.AddNode(element);
        }

        public void RemoveTiles(Transform2D transform)
        {
            // Collision for removal. Bit smaller than one tile to prevent overlap with adjacent tiles
            var aabb = new AABB(transform.Translation, Constants.TILE_SIZE - 0.1f, Constants.TILE_SIZE - 0.1f);

            List<Element<Entity>> toRemove = new List<Element<Entity>>();
            this.quadtree.QueryAABB((element) =>
            {
                toRemove.Add(element);
                return true;
            }, ref aabb);

            foreach (var element in toRemove)
            {
                this.quadtree.RemoveNode(element);
            }
        }
    }
}

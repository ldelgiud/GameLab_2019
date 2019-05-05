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
using Hazmat.Utilities.Extensions;

namespace Hazmat.Utilities
{
    class TileMap
    {
        World world = new World();
        TileModelResourceManager tileModelResourceManager;

        public QuadTree<Entity> quadtree = new QuadTree<Entity>(new AABB(Constants.BOTTOM_LEFT_CORNER, Constants.TOP_RIGHT_CORNER), 10, 7);

        public TileMap(GraphicsDevice graphicsDevice, string atlasPath)
        {
            this.tileModelResourceManager = new TileModelResourceManager(graphicsDevice, atlasPath);
            this.tileModelResourceManager.Manage(this.world);
        }

        public void AddTile(Transform3D transform, TileModelInfo info, String name)
        {
            var entity = this.world.CreateEntity();
            entity.Set(new Transform3DComponent(transform));
            entity.Set(new ManagedResource<TileModelInfo, TileModelAlias>(info));
            entity.Set(new NameComponent() { name = name});

            var element = new Element<Entity>(new AABB(transform.Translation.ToVector2(), transform.Scale.X, transform.Scale.Y))
            {
                Value = entity
            };
            this.quadtree.AddNode(element);
        }

        public void RemoveTiles(Transform2D transform)
        {
            // Collision for removal. Bit smaller than one tile to prevent overlap with adjacent tiles
            var aabb = new AABB(transform.Translation, Constants.TILE_SIZE - 0.01f, Constants.TILE_SIZE - 0.01f);

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

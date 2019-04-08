using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Spine;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.ResourceManagers
{
    class AtlasTextureResourceManager : AResourceManager<Texture2DInfo, AtlasTextureAlias>
    {
        Atlas atlas;

        public AtlasTextureResourceManager(GraphicsDevice graphicsDevice, String path)
        {
            this.atlas = new Atlas(@"Content\" + path + ".atlas", new XnaTextureLoader(graphicsDevice));
        }

        protected override AtlasTextureAlias Load(Texture2DInfo info)
        {
            var region = this.atlas.FindRegion(info.name);

            return new AtlasTextureAlias (
                (Texture2D)this.atlas.pages[0].rendererObject,
                new Rectangle(region.x, region.y, region.width, region.height)
                );
        }

        protected override void OnResourceLoaded(in Entity entity, Texture2DInfo info, AtlasTextureAlias resource)
        {
            info.bounds = resource.bounds;

            if (info.scale.X < 0 && info.scale.Y < 0)
            {
                info.scale = new Vector2(info.width / resource.bounds.Width, info.height / resource.bounds.Height);
            }

            entity.Set(new Texture2DComponent(resource.value, info));
        }
    }
}

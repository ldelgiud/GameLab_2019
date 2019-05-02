using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
{
    sealed class TextureResourceManager : AResourceManager<Texture2DInfo, Texture2DAlias>
    {
        ContentManager contentManager;

        public TextureResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override Texture2DAlias Load(Texture2DInfo info)
        {
            return new Texture2DAlias(this.contentManager.Load<Texture2D>(info.name), info.bounds);
        }

        protected override void OnResourceLoaded(in Entity entity, Texture2DInfo info, Texture2DAlias resource)
        {
            info.bounds = resource.value.Bounds;

            // Set scale from width/height
            if (info.scale.X < 0 && info.scale.Y < 0)
            {
                info.scale = new Vector2(info.width / resource.value.Bounds.Width, info.height / resource.value.Bounds.Height);
            }

            entity.Set(new Texture2DComponent(resource.value, info));
        }
    }
}

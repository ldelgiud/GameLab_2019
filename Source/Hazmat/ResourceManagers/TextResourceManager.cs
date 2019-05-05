using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using DefaultEcs;
using DefaultEcs.Resource;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
{
    class TextResourceManager : AResourceManager<TextInfo, TextAlias>
    {
        ContentManager contentManager;

        public TextResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override TextAlias Load(TextInfo info)
        {
            return new TextAlias(this.contentManager.Load<SpriteFont>(info.font));
        }

        protected override void OnResourceLoaded(in Entity entity, TextInfo info, TextAlias resource)
        {
            entity.Set(new TextComponent(info, info.text, resource.font));
        }
    }
}

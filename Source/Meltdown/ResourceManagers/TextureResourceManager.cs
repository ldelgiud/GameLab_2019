using System.Diagnostics;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using Meltdown.Components;

namespace Meltdown.ResourceManagers
{
    sealed class TextureResourceManager : AResourceManager<string, Texture2D>
    {
        ContentManager contentManager;

        public TextureResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override Texture2D Load(string info)
        {
            return this.contentManager.Load<Texture2D>(info);
        }

        protected override void OnResourceLoaded(in Entity entity, string info, Texture2D resource)
        {
            entity.Set(new TextureComponent() { value = resource });
        }
    }
}

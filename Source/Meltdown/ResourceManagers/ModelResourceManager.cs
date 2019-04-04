using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.ResourceManagers
{
    sealed class ModelResourceManager : AResourceManager<string, ModelAlias>
    {
        ContentManager contentManager;

        public ModelResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override ModelAlias Load(string info)
        {
            return new ModelAlias() { value = this.contentManager.Load<Model>(info) };
        }

        protected override void OnResourceLoaded(in Entity entity, string info, ModelAlias resource)
        {
            entity.Set(new ModelComponent(resource.value));
        }
    }
}

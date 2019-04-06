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
    sealed class ModelResourceManager : AResourceManager<ModelInfo, ModelAlias>
    {
        ContentManager contentManager;

        public ModelResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override ModelAlias Load(ModelInfo info)
        {
            return new ModelAlias() { value = this.contentManager.Load<Model>(info.name) };
        }

        protected override void OnResourceLoaded(in Entity entity, ModelInfo info, ModelAlias resource)
        {
            entity.Set(new ModelComponent() { value = resource.value, info = info });
        }
    }
}

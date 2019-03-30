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
    sealed class ModelResourceManager : AResourceManager<string, ModelWrapper>
    {
        ContentManager contentManager;

        public ModelResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        protected override ModelWrapper Load(string info)
        {
            return new ModelWrapper() { value = this.contentManager.Load<Model>(info) };
        }

        protected override void OnResourceLoaded(in Entity entity, string info, ModelWrapper resource)
        {
            entity.Set(new ModelComponent(resource.value));
        }
    }
}

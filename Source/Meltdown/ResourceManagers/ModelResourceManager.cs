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
            
            // Add ModelLoader instance
            
        }

        protected override ModelAlias Load(ModelInfo info)
        {
            var model = this.contentManager.Load<Model>(info.name);

            if (info.textureName != null)
            {
                var texture = this.contentManager.Load<Texture2D>(info.textureName);
                foreach (var mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = texture;
                    }
                }
            }

            return new ModelAlias() { value =  model };
        }

        protected override void OnResourceLoaded(in Entity entity, ModelInfo info, ModelAlias resource)
        {
            entity.Set(new ModelComponent() { value = resource.value, info = info });
        }
    }
}

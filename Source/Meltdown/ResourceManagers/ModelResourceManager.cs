using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using Meltdown.Graphics;
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
            var model = this.contentManager.Load<Model>(info.name);
            
            if (info.textureName != null)
            {
                Texture2D texture;
                // If texture is inside .fbx
                if (info.textureName == "")
                {
                    texture = ((BasicEffect)model.Meshes[0].Effects[0]).Texture;
                }
                else
                {
                    texture = this.contentManager.Load<Texture2D>(info.textureName);
                }
                
                // Textured base effect
                if (info.standardEffect == null)
                {
                    foreach (var mesh in model.Meshes)
                    {
                        foreach (var part in mesh.MeshParts)
                        {
                            BasicEffect eff = (BasicEffect)part.Effect;
                            eff.TextureEnabled = true;
                            eff.Texture = texture;
                        }
                    }
                }
                else // Textured custom shader effect
                {
                    
                    foreach (var mesh in model.Meshes)
                    {
                        foreach (var part in mesh.MeshParts)
                        {
                            part.Effect = info.standardEffect;
                            part.Effect.Parameters["Texture"].SetValue(texture);
                        }
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

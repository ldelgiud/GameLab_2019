using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs.Resource;
using DefaultEcs;

using System;
using System.Diagnostics;

using tainicom.Aether.Animation;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
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
                var texture = this.contentManager.Load<Texture2D>(info.textureName);
                
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
                    var effect = info.standardEffect.Clone();
                    foreach (var mesh in model.Meshes)
                    {
                        foreach (var part in mesh.MeshParts)
                        {
                            part.Effect = effect;
                            part.Effect.Parameters["Texture"].SetValue(texture);
                        }
                    }
                }
            }
            
            return new ModelAlias() { value =  model };
        }

        protected override void OnResourceLoaded(in Entity entity, ModelInfo info, ModelAlias resource)
        {
            var animations = resource.value.GetAnimations();

            if (animations != null)
            {
                entity.Set(new ModelAnimationComponent(animations));

                if (info.animation != null)
                {
                    animations.SetClip(animations.Clips[info.animation]);
                }
            }
            
            entity.Set(new ModelComponent() { value = resource.value, info = info });
        }
    }
}

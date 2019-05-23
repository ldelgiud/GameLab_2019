using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using Hazmat.Components;

using Hazmat.Utilities;

namespace Hazmat.Components
{
    public struct ModelComponent
    {
        public Model value;
        public ModelInfo info;

        public ModelComponent(Model model, ModelInfo info)
        {
            this.value = model;
            this.info = info;
        }

        public void UpdateEffects(Effect effect, float time)
        {
            if (info.updateTimeEffect)
            {
                effect.Parameters["time"].SetValue(time);
            }
        }

        /// <summary>
        /// Apply a new shader to the model.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="updateTimeEffect"></param>
        public void ApplyEffect(Effect effect, bool updateTimeEffect = false)
        {
            foreach(var mesh in value.Meshes)
            {
                foreach(var part in mesh.MeshParts)
                {
                    part.Effect = effect.Clone();
                }
            }

            info.cachedUpdateTimeEffect = updateTimeEffect;
            info.updateTimeEffect = updateTimeEffect;
        }

        /// <summary>
        /// Restore the standard shader of this model.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="updateTimeEffect"></param>
        public void RestoreStandardEffect()
        {
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = info.standardEffect.Clone();
                }
            }

            info.updateTimeEffect = info.cachedUpdateTimeEffect;
        }

        public void EnableDamageEffect(bool enable = true)
        {
            //if (enable) ApplyEffect(Hazmat.Instance.Content.Load<Effect>("shaders/blink"));
            //else RestoreStandardEffect();
            Debug.WriteLine("Value for enable damage effect is: " + enable);
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    //if (enable) part.Effect = part.Effect.Clone();
                    part.Effect.Parameters["ActivateBlink"].SetValue(enable);
                }
            }
        }

        /// <summary>
        /// Enables blink effext for all childrens of an entity (used for player mainly).
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="enable"></param>
        public void EnableDamageEffectForChildren(Entity entity, bool enable = true)
        {
            foreach(Entity ent in entity.GetChildren())
            {
                if (ent.Has<ModelComponent>())
                {
                    ref ModelComponent model = ref ent.Get<ModelComponent>();
                    model.EnableDamageEffect(enable);
                }
            }
        }

        /// <summary>
        /// Apply glowing to this model. Assumes that it has toon shader. Call only when model has this shader.
        /// </summary>
        public void EnableToonGlow()
        {
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect.Parameters["enableGlowing"].SetValue(true);
                    info.updateTimeEffect = true;
                }
            }
        }

        /// <summary>
        /// Disable glowing for this model. Assumes that it has toon shader. Call only when model has this shader.
        /// </summary>
        public void DisableToonGlow()
        {
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect.Parameters["enableGlowing"].SetValue(false);
                    info.updateTimeEffect = false;
                }
            }
        }

        public void ChangeParameter(string parameterID, Vector4 parameterValue)
        {
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    if (part.Effect.Parameters[parameterID] != null)
                    {
                        part.Effect.Parameters[parameterID].SetValue(parameterValue);
                    }
                }
            }
        }

        public void ChangeParameter(string parameterID, float parameterValue)
        {
            foreach (var mesh in value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    if (part.Effect.Parameters[parameterID] != null)
                    {
                        part.Effect.Parameters[parameterID].SetValue(parameterValue);
                    }
                }
            }
        }
    }
}

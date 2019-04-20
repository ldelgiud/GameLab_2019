using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hazmat.Utilities;

namespace Hazmat.Components
{
    struct ModelComponent
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


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Hazmat.Utilities;

namespace Hazmat.Components
{
    struct Texture2DComponent
    {
        public Texture2D value;
        public Texture2DInfo info;
        
        public Texture2DComponent(Texture2D value, Texture2DInfo info)
        {
            this.value = value;
            this.info = info;
        }


        /// <summary>
        /// Add temporary effect to this texture (e.g. glowing, damage).
        /// </summary>
        /// <param name="tempEffect"></param>
        public void SetTemporaryEffect(Effect tempEffect, String tempParameterToUpdate = null)
        {
            this.info.tempParameterToUpdate = tempParameterToUpdate;
            this.info.tempEffect = tempEffect;
        }

        /// <summary>
        /// Removes temporary effect. Now the standard effect will be used. 
        /// If no standard effect defined normal spriteBatch effect is used.
        /// </summary>
        public void RemoveTemporaryEffect()
        {
            this.info.tempParameterToUpdate = null;
            this.info.tempEffect = null;
        }

        /// <summary>
        /// Return effect to be used.
        /// </summary>
        /// <returns></returns>
        public Effect Effect()
        {
            return this.info.tempEffect ?? this.info.standardEffect ?? null;
        }

        /// <summary>
        /// Update parameter for current effect. 
        /// </summary>
        /// <param name="time"></param>
        public void UpdateEffects(float time)
        {
            Effect eff = this.Effect();
            if (this.info.tempParameterToUpdate != null)
                eff.Parameters[this.info.tempParameterToUpdate].SetValue(time);
            else if (this.info.standardParameterToUpdate != null)
                eff.Parameters[this.info.standardParameterToUpdate].SetValue(time);
            
        }

    }
}

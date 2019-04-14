using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Meltdown.Utilities;

namespace Meltdown.Components
{
    struct Texture2DComponent
    {
        public Texture2D value;
        public Texture2DInfo info;

        public Effect standardEffect;
        public Effect tempEffect;

        // For variable that changes over time
        public String standardParameterToUpdate;
        public String tempParameterToUpdate;

        public Texture2DComponent(Texture2D value, Texture2DInfo info, Effect standardEffect = null, Effect tempEffect = null, String standardParameterToUpdate = null)
        {
            this.value = value;
            this.info = info;

            this.standardEffect = standardEffect;
            this.tempEffect = tempEffect;

            this.standardParameterToUpdate = standardParameterToUpdate ?? null;
            this.tempParameterToUpdate = null;
        }


        /// <summary>
        /// Add temporary effect to this texture (e.g. glowing, damage).
        /// </summary>
        /// <param name="tempEffect"></param>
        public void SetTemporaryEffect(Effect tempEffect, String tempParameterToUpdate = null)
        {
            this.tempParameterToUpdate = tempParameterToUpdate;
            this.tempEffect = tempEffect;
        }

        /// <summary>
        /// Removes temporary effect. Now the standard effect will be used. 
        /// If no standard effect defined normal spriteBatch effect is used.
        /// </summary>
        public void RemoveTemporaryEffect()
        {
            this.tempParameterToUpdate = null;
            this.tempEffect = null;
        }

        /// <summary>
        /// Return effect to be used.
        /// </summary>
        /// <returns></returns>
        public Effect Effect()
        {
            return tempEffect ?? standardEffect ?? null;
        }

        /// <summary>
        /// Update parameter for current effect. 
        /// </summary>
        /// <param name="time"></param>
        public void UpdateEffects(float time)
        {
            Effect eff = this.Effect();
            if (tempParameterToUpdate != null)
                eff.Parameters[tempParameterToUpdate].SetValue(time);
            else if (standardParameterToUpdate != null)
                eff.Parameters[standardParameterToUpdate].SetValue(time);
            
        }

    }
}

using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;
using System.Diagnostics;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class DamageEffectSystem : AEntitySystem<Time>
    {
        public DamageEffectSystem(World world) : base(
            world.GetEntities()
            .With<DamageEffectComponent>()
            .Build()
            ) { }

        protected override void Update(Time state, in Entity entity)
        {
            ref DamageEffectComponent damageEffect = ref entity.Get<DamageEffectComponent>();

            if (damageEffect.currentTimeEffect < damageEffect.totalTimeEffect)
            {
                damageEffect.IncrementCurrentTimeEffect(state.Delta);
            }
            else
            {
                ResetModel(entity);
            }
        }

        public void ResetModel(Entity entity)
        {
            ref ModelComponent model = ref entity.Get<ModelComponent>();
            model.EnableDamageEffect(false);
            model.EnableDamageEffectForChildren(entity, false);
            entity.Remove<DamageEffectComponent>();
        }

    }

}


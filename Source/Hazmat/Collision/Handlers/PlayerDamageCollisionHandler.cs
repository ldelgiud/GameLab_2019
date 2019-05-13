using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DefaultEcs;

using Hazmat.Collision;
using Hazmat.Components;
using Hazmat.Music;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Collision.Handlers
{
    class PlayerDamageCollisionHandler : CollisionHandler
    {
        SoundManager soundManager
        {
            get
            {
                return Hazmat.Instance.SoundManager;
            }
        }
        Energy energy;

        public PlayerDamageCollisionHandler(World world, Energy energy) : base(
            new Type[] {typeof(PlayerComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            new Type[] {typeof(DamageComponent), typeof(AABBComponent), typeof(AllianceMaskComponent)},
            true
            )
        {
            this.energy = energy;
        }
        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
            switch(type) {
                case CollisionType.Start:
                    Alliance playerAlliance = collider.Get<AllianceMaskComponent>().alliance;
                    Alliance collideeAlliance = collidee.Get<AllianceMaskComponent>().alliance;
                    if (((int)playerAlliance | (int)collideeAlliance) != (int)playerAlliance)
                    {

                        ref StatsComponent playerStats = ref collider.Get<StatsComponent>();
                        ref ModelComponent playerModel = ref collider.Get<ModelComponent>();

                        float timeDamageEffect = 0.2f;

                        if (collider.Has<DamageEffectComponent>())
                        {
                            ref DamageEffectComponent damageEffectPlayer = ref collider.Get<DamageEffectComponent>();
                            damageEffectPlayer.Initialize(timeDamageEffect);
                        }
                        else
                        {
                            playerModel.EnableDamageEffect();
                            playerModel.EnableDamageEffectForChildren(collider);
                            DamageEffectComponent damageEffect = new DamageEffectComponent();
                            damageEffect.Initialize(timeDamageEffect);
                            collider.Set(damageEffect);
                        }
                        

                        energy.CurrentEnergy -= (collidee.Get<DamageComponent>().Damage * (1f - playerStats.Defense));

                        if (energy.CurrentEnergy == 0)
                        {
                            soundManager.PlaySoundEffect(soundManager.MatDying);
                        } else
                        {
                            soundManager.PlaySoundEffect(soundManager.MatHit);
                        }
                        collidee.Delete();

                        GamePad.SetVibration(0, 1, 1);
                        GamePad.SetVibration(0, 0, 0);
                    }
                    break;
            }

            
            
        }
    }
}

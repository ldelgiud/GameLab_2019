using System;
using System.Diagnostics;

using DefaultEcs;
using Hazmat.Utilities.Extensions;
using Microsoft.Xna.Framework;

namespace Hazmat.Components
{
    public class StatsComponent
    {

        private Entity PlayerEntity;
        private int ArmorLevel;
        private int DamageLevel;
        private Entity SmallTank; // reference needed when placing the big tank

        public bool CurrentlyDisplayingOtherPowerUp; // Used in PowerUpPickUpCollHandler - avoid getting power up 2 times

        public float Speed;
        public float Damage;
        public float Defense; // in percentage

        public StatsComponent(float speed, float damage, Entity entity)
        {
            this.Speed = speed;
            this.Damage = damage;
            this.PlayerEntity = entity;
            
            this.Defense = 0;
            this.ArmorLevel = 0;
            this.DamageLevel = 0;
        }

        public void UpgradeSpeed(int amount = 2)
        {
            this.Speed += amount;
        }

        public void UpgradeDamage(int amount = 10)
        {
            UpgradeWeapon();
            this.Damage += amount;
        }

        public void UpgradeDefense(float amount = 0.1f)
        {
            UpgradeArmor();
            this.Defense += amount;
            this.Defense = MathHelper.Clamp(this.Defense, 0f, 0.3f);
        }

        // Upgrade Damage
        public void UpgradeWeapon()
        {
            if (PlayerEntity.Has<WeaponComponent>())
            {
                ref WeaponComponent weapon = ref PlayerEntity.Get<WeaponComponent>();
                ref SmallGunComponent smallGun =  ref weapon.weapon.Get<SmallGunComponent>();
                
                switch (this.DamageLevel)
                {
                    case 0:
                        smallGun.ChangeProjectiles("MatProjectile_02");
                        PlayerEntity.SetAttachment(
                            @"weapons\MED_WP_MatGunBasic_upgrade1",
                            @"weapons\TEX_WP_MatGunBasic_01",
                            rotation: new Vector3(0f,0f,MathHelper.Pi),
                            position: new Vector3(-1.15f, -1f, 0.25f)
                            );
                        break;
                    case 1:
                        smallGun.ChangeProjectiles("MatProjectile_03");
                        PlayerEntity.SetAttachment(
                            @"weapons\MED_WP_MatGunBasic_upgrade2",
                            @"weapons\TEX_WP_MatGunBasic_01",
                            rotation: new Vector3(0f, 0f, MathHelper.Pi),
                            position: new Vector3(-1.15f, -1f, 0.25f)
                            );
                        break;
                    case 2:
                        smallGun.ChangeProjectiles("MatProjectile_04");
                        break;
                }
                PlayerEntity.SetModelAnimation("Take 001");
                PlayerEntity.SyncModelAnimation();
                this.DamageLevel++;
            }
        }

            // Upgrade Armor
            public void UpgradeArmor()
        {
            switch (this.ArmorLevel)
            {
                case 0:
                    PlayerEntity.SetAttachment(
                        @"characters\armor\MED_AR_MatHelmet_L_01", 
                        @"characters\armor\TEX_AR_TanksMasksBP_01",
                        position: new Vector3(0f, 0, 0.5f)
                        );
                    break;
                case 1:
                    SmallTank = PlayerEntity.SetAttachment(
                        @"characters\armor\MED_AR_MatAirTank_01",
                        @"characters\armor\TEX_AR_TanksMasksBP_01",
                        position: new Vector3(-1.2f, 0, 2.1f)
                       // scale: new Vector3(0.6f)
                        );
                    break;
                case 2:
                    SmallTank.Delete();
                    PlayerEntity.SetAttachment(
                        @"characters\armor\MED_AR_MatAirTank_L_01", 
                        @"characters\armor\TEX_AR_TanksMasksBP_01",
                        position: new Vector3(-1.2f, 0, 2f)
                        );
                    break;
            }
            PlayerEntity.SetModelAnimation("Take 001");
            PlayerEntity.SyncModelAnimation();
            this.ArmorLevel++;
        }

    }
}

using System;
using System.Diagnostics;

using DefaultEcs;
using Hazmat.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hazmat.Components
{
    public class StatsComponent
    {
        private ModelComponent GunModelComponent; // used for changing color of gun
        private ModelComponent[] GunAttachementModelComponent;
        private Entity PlayerEntity;
        private int ArmorLevel;
        private int DamageLevel;
        private Entity SmallTank; // reference needed when placing the big tank

        public bool CurrentlyDisplayingOtherPowerUp; // Used in PowerUpPickUpCollHandler - avoid getting power up 2 times

        public float Speed;
        public float Damage;
        public float Defense; // in percentage

        public Texture2D texture2DGreenGun;
        public Texture2D texture2DOrangeGun;
        public Texture2D texture2DWhiteGun;

        public StatsComponent(float speed, float damage, Entity playerEntity, ModelComponent modelComponent)
        {
            this.Speed = speed;
            this.Damage = damage;
            this.PlayerEntity = playerEntity;
            this.GunModelComponent = modelComponent;
            this.GunAttachementModelComponent = new ModelComponent[2];

            this.Defense = 0;
            this.ArmorLevel = 0;
            this.DamageLevel = 0;

            this.texture2DGreenGun = Hazmat.Instance.Content.Load<Texture2D>(@"weapons\TEX_WP_MatGunBasic_01Green");
            this.texture2DOrangeGun = Hazmat.Instance.Content.Load<Texture2D>(@"weapons\TEX_WP_MatGunBasic_01Orange");
            this.texture2DWhiteGun = Hazmat.Instance.Content.Load<Texture2D>(@"weapons\TEX_WP_MatGunBasic_01White");
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
                        // Projectiles
                        smallGun.ChangeProjectiles("MatProjectile_02");
                    
                        // Gun Attachement
                        Entity attachement = PlayerEntity.SetAttachment(
                            @"weapons\MED_WP_MatGunBasic_upgrade1",
                            @"weapons\TEX_WP_MatGunBasic_01",
                            rotation: new Vector3(0f,0f,MathHelper.Pi),
                            position: new Vector3(-1.15f, -1f, 0.25f)
                            );

                        //Color change
                        GunAttachementModelComponent[0] = attachement.Get<ModelComponent>();
                        GunAttachementModelComponent[0].ChangeParameter("Texture", texture2DGreenGun);
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientColor", new Vector4(0, 1, 0, 1));
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientIntensity",0.8f);

                        //GunModelComponent.ChangeParameter("AmbientColor", new Vector4(0, 1, 0, 1));
                        //GunModelComponent.ChangeParameter("AmbientIntensity", 0.8f);

                        GunModelComponent.ChangeParameter("Texture", texture2DGreenGun);

                        break;
                    case 1:
                        // Projectiles
                        smallGun.ChangeProjectiles("MatProjectile_03");

                        // Gun Attachement
                        Entity attachement2 = PlayerEntity.SetAttachment(
                            @"weapons\MED_WP_MatGunBasic_upgrade2",
                            @"weapons\TEX_WP_MatGunBasic_01",
                            rotation: new Vector3(0f, 0f, MathHelper.Pi),
                            position: new Vector3(-1.15f, -1f, 0.25f)
                            );

                        // Color change
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientColor", new Vector4(1, 0.5f, 0, 1));
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientIntensity", 0.7f);

                        GunAttachementModelComponent[1] = attachement2.Get<ModelComponent>();
                        GunAttachementModelComponent[1].ChangeParameter("Texture", texture2DOrangeGun);

                        //GunAttachementModelComponent[1].ChangeParameter("AmbientColor", new Vector4(1, 0.5f, 0, 1));
                        //GunAttachementModelComponent[1].ChangeParameter("AmbientIntensity", 0.7f);

                        //GunModelComponent.ChangeParameter("AmbientColor", new Vector4(1, 0.5f, 0, 1));
                        //GunModelComponent.ChangeParameter("AmbientIntensity", 0.7f);
                        GunAttachementModelComponent[0].ChangeParameter("Texture", texture2DOrangeGun);
                        GunModelComponent.ChangeParameter("Texture", texture2DOrangeGun);


                        break;
                    case 2:
                        smallGun.ChangeProjectiles("MatProjectile_04");

                        // Color change
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientColor", new Vector4(1, 1, 1, 1));
                        //GunAttachementModelComponent[0].ChangeParameter("AmbientIntensity", 0.7f);

                        //GunAttachementModelComponent[1].ChangeParameter("AmbientColor", new Vector4(1, 1, 1, 1));
                        //GunAttachementModelComponent[1].ChangeParameter("AmbientIntensity", 1f);

                        //GunModelComponent.ChangeParameter("AmbientColor", new Vector4(1, 1, 1, 1));
                        //GunModelComponent.ChangeParameter("AmbientIntensity", 0.7f);

                        GunAttachementModelComponent[1].ChangeParameter("Texture", texture2DWhiteGun);
                        GunAttachementModelComponent[0].ChangeParameter("Texture", texture2DWhiteGun);
                        GunModelComponent.ChangeParameter("Texture", texture2DWhiteGun);

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

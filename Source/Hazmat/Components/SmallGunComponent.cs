using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Hazmat.Collision;
using Hazmat.Utilities;
using tainicom.Aether.Physics2D.Collision;

using Hazmat.GameElements.Shooting;
using Hazmat.Graphics;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Components
{
    class SmallGunComponent : AGun
    {

        public string ProjectileSkinName { get; private set; }
        public string ProjectileAnimationName { get; private set; }


        public SmallGunComponent(
            float damage, 
            float projectileSpeed, 
            float radiusRange,
            float reloadTime, 
            string projectileSkin,
            string projectileAnimation,
            Alliance alliance)
        {
            this.damage = damage;
            this.projectileSpeed = projectileSpeed;
            this.radiusRange = radiusRange;
            this.reloadTime = reloadTime;
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
            this.alliance = alliance;

            this.ProjectileSkinName = projectileSkin;// "MatProjectile_01";
            this.ProjectileAnimationName = projectileAnimation;// "MatProjectile_01"; //taken from items\SPS_Projectiles
        }

        public override void Shoot(float absoluteTime, Transform3D transform, Vector2 direction)
        {
            if ((absoluteTime - timeLastShot) < reloadTime) { return; }

            // Play Sound Effect
            Hazmat.Instance.SoundManager.PlaySoundEffect(Hazmat.Instance.SoundManager.Shooting_Sfx);

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            
            Entity entity = SpawnHelper.SpawnBullet(transform.Translation, direction);

            entity.Set(new VelocityComponent(direction * this.projectileSpeed));
            entity.Set(new ManagedResource<SpineAnimationInfo, SkeletonDataAlias>(
                new SpineAnimationInfo(
                    @"items\SPS_Projectiles",
                    new SkeletonInfo(3f, 3f, skin: this.ProjectileSkinName, translation: new Vector3(0, 0, 1f)),
                    new AnimationStateInfo(this.ProjectileAnimationName, true)
                )
            ));

            entity.Set(new DamageComponent(this.damage + this.additionalDamage)); // added for collision handling
            entity.Set(new NameComponent() { name = "bullet" });
            entity.Set(new TTLComponent(Constants.TTL_BULLET));
            entity.Set(new AllianceMaskComponent(this.alliance));
            timeLastShot = absoluteTime;
        }

        public void ChangeProjectiles(string projSkinName, string projAnimName = "MatProjectile_01")
        {
            this.ProjectileSkinName = projSkinName;
            this.ProjectileAnimationName = projAnimName;
        }
    }
}

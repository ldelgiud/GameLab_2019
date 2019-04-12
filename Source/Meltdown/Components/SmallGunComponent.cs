using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Meltdown.Collision;
using Meltdown.Utilities;
using tainicom.Aether.Physics2D.Collision;

using Meltdown.GameElements.Shooting;
using Meltdown.Graphics;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Components
{
    class SmallGunComponent : AGun
    {
        
        public SmallGunComponent(
            float damage, 
            float projectileSpeed, 
            float radiusRange,
            float reloadTime, 
            string projTex,
            Alliance alliance)
        {
            this.damage = damage;
            this.projectileSpeed = projectileSpeed;
            this.radiusRange = radiusRange;
            this.reloadTime = reloadTime;
            this.projectileTexture = projTex;
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
            this.alliance = alliance;
        }

        public override void Shoot(float absoluteTime, Transform2D transform, Vector2 direction)
        {
            if ((absoluteTime - timeLastShot) < reloadTime) { return; }
            direction.Normalize();

            Entity entity = SpawnHelper.SpawnBullet(transform.Translation, direction);
            
            entity.Set(new VelocityComponent(direction * this.projectileSpeed));
            entity.Set(new ManagedResource<Texture2DInfo, AtlasTextureAlias>(new Texture2DInfo(@"static_sprites/SPT_WP_Projectile_01", 0.4f, 0.4f, rotation: -MathF.PI / 2)));
            entity.Set(new DamageComponent(this.damage)); // added for collision handling
            entity.Set(new NameComponent() { name = "bullet" });
            entity.Set(new TTLComponent(Constants.TTL_BULLET));
            entity.Set(new AllianceMaskComponent(this.alliance));
            timeLastShot = absoluteTime;
        }
        
    }
}

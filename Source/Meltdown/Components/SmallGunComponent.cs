using System;
using System.Diagnostics;

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

        public override void Shoot(float absoluteTime, WorldTransformComponent gunTransform, Vector2 direction)
        {
            direction.Normalize();
            if ((absoluteTime - timeLastShot) < reloadTime) { return; }
            var globalTransform = gunTransform.value.GlobalTransform;
            Vector3 position = globalTransform.Translation();

            Entity entity = SpawnHelper.SpawnBullet(position, direction);

            entity.Set(new VelocityComponent(direction * this.projectileSpeed));
            entity.Set(new ManagedResource<string, Texture2D>(this.projectileTexture));
            entity.Set(new DamageComponent(this.damage)); // added for collision handling
            entity.Set(new BoundingBoxComponent(20, 20, 0));
            entity.Set(new NameComponent() { name = "bullet" });
            entity.Set(new TTLComponent(Constants.TTL_BULLET));
            entity.Set(new AllianceMaskComponent(this.alliance));
            timeLastShot = absoluteTime;
        }
    }
}

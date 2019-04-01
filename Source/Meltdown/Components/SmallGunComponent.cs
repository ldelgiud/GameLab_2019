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
            Texture2D projTex,
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
            if ((absoluteTime - timeLastShot) < reloadTime) { return; }
            var globalTransform = gunTransform.value.GlobalTransform;
            Vector3 position = globalTransform.Translation();

            SpawnHelper.SpawnBullet(
                position, 
                direction, 
                this.projectileSpeed, 
                this.damage, 
                this.alliance);

            
            timeLastShot = absoluteTime;
        }
        
    }
}

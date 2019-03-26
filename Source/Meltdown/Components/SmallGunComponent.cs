using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Meltdown.GameElements.Shooting;

namespace Meltdown.Components
{
    class SmallGunComponent : AGun
    {
        public SmallGunComponent(float projectileSpeed, float radiusRange, float reloadTime, Texture2D projTex)
        {
            projectile = new Projectile(projectileSpeed, radiusRange, projTex);
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
        }

        public override void Shoot(float absoluteTime, WorldTransformComponent transform, Vector2 direction, World world)
        {
            if (absoluteTime - timeLastShot < reloadTime) { return; }

            direction.Normalize();

            float rotation = MathF.Atan2(direction.Y, direction.X);
            Debug.WriteLine("Rotation: " + rotation + ", direction: " + direction);

            var entity = world.CreateEntity();
            entity.Set(new WorldTransformComponent(transform.Position, MathHelper.PiOver2 - rotation, Vector2.One * 0.03f));
            entity.Set(new VelocityComponent(direction * projectile.speed));
            entity.Set(new TextureComponent { value = projectile.projTex });
            entity.Set(new BoundingBoxComponent(50, 50, 0));

            timeLastShot = absoluteTime;
        }
        
    }
}

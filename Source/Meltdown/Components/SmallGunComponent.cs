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

namespace Meltdown.Components
{
    class SmallGunComponent : AGun
    {
        public SmallGunComponent(float damage, float projectileSpeed, float radiusRange, float reloadTime, Texture2D projTex)
        {
            projectile = new ProjectileComponent(damage, projectileSpeed, radiusRange, projTex);
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
        }

        public override void Shoot(float absoluteTime, WorldTransformComponent transform, Vector2 direction, World world)
        {
            if (absoluteTime - timeLastShot < reloadTime) { return; }

            direction.Normalize();

            float rotation = MathF.Atan2(direction.Y, direction.X);
            //Debug.WriteLine("Rotation: " + rotation + ", direction: " + direction);
            
            var entity = world.CreateEntity();
            entity.Set(new WorldTransformComponent(
                new Transform(
                    transform.value.position,
                    Vector3.Zero,
                    Vector3.One * 0.1f
                    )
                )
            );
            entity.Set(new VelocityComponent(direction * projectile.speed));
            entity.Set(projectile); // added for collision handling
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new TextureComponent { value = projectile.projTex });
            entity.Set(new BoundingBoxComponent(20, 20, 0));
            
            SpawnHelper.quadtree.AddNode(element);


            timeLastShot = absoluteTime;
        }
        
    }
}

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

            //Bounding box stuff
            AABB aabb = new AABB()
            {
                LowerBound = new Vector2(-10, -10),
                UpperBound = new Vector2(10, 10)
            };
            Element<Entity> element = new Element<Entity>(aabb) { Value = entity };
            element.Span.LowerBound += transform.Position;
            element.Span.UpperBound += transform.Position;

            entity.Set(new WorldTransformComponent(transform.Position, MathHelper.PiOver2 - rotation, Vector2.One * 0.03f));
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

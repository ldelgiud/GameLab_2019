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
        public SmallGunComponent(float damage, float projectileSpeed, float radiusRange, float reloadTime, Texture2D projTex)
        {
            projectile = new ProjectileComponent(damage, projectileSpeed, radiusRange, projTex);
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
        }

        public override void Shoot(float absoluteTime, WorldTransformComponent gunTransform, Vector2 direction, World world)
        {
            if (absoluteTime - timeLastShot < reloadTime) { return; }

            float rotation = MathF.Atan2(-direction.Y, direction.X);

            var globalTransform = gunTransform.value.GlobalTransform;
            
            var position = globalTransform.Translation();
            var entity = world.CreateEntity();
            var projectileTransform = new WorldTransformComponent(
                new Transform(
                    position,
                    Vector3.Zero,
                    Vector3.One * 0.05f
                    )
                );

            entity.Set(projectileTransform);
            projectileTransform.value.Rotate(0, 0, rotation + MathHelper.PiOver2);

            var aabb = new AABB(new Vector2(-0.1f, -0.1f), new Vector2(0.1f, 0.1f));

            var element = new Element<Entity>(aabb);
            element.Span.LowerBound += position.ToVector2();
            element.Span.UpperBound += position.ToVector2();
            element.Value = entity;

            entity.Set(new VelocityComponent(direction * this.projectile.speed));
            entity.Set(projectile); // added for collision handling
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new TextureComponent { value = projectile.projTex });
            entity.Set(new BoundingBoxComponent(20, 20, 0));
            entity.Set(new NameComponent() { name = "bullet" });
            entity.Set(new TTLComponent(10f));
            SpawnHelper.quadtree.AddNode(element);


            timeLastShot = absoluteTime;
        }
        
    }
}

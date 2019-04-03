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
        public SmallGunComponent(float damage, float projectileSpeed, float radiusRange, float reloadTime)
        {
            projectile = new ProjectileComponent(damage, projectileSpeed, radiusRange);
            this.reloadTime = reloadTime;
            this.timeLastShot = 0f;
        }

        public override void Shoot(float absoluteTime, Transform2D transform, Vector2 direction, World world)
        {
            if (absoluteTime - timeLastShot < reloadTime) { return; }

            direction.Normalize();

            float rotation = MathF.Atan2(direction.Y, direction.X);
            //Debug.WriteLine("Rotation: " + rotation + ", direction: " + direction);
            
            var entity = world.CreateEntity();
            entity.Set(new Transform2DComponent(new Transform2D(transform.Translation)));
            entity.Set(new WorldSpaceComponent());

            var aabb = new AABB(new Vector2(-0.1f, -0.1f), new Vector2(0.1f, 0.1f));

            var element = new Element<Entity>(aabb);
            element.Span.LowerBound += transform.Translation;
            element.Span.UpperBound += transform.Translation;
            element.Value = entity;

            entity.Set(new VelocityComponent(direction * this.projectile.speed));
            entity.Set(projectile); // added for collision handling
            entity.Set(new AABBComponent(SpawnHelper.quadtree, aabb, element, false));
            entity.Set(new ManagedResource<Texture2DInfo, Texture2D>(new Texture2DInfo(@"shooting\bullet", null, null, 0.2f, 0.2f)));
            
            SpawnHelper.quadtree.AddNode(element);


            timeLastShot = absoluteTime;
        }
        
    }
}

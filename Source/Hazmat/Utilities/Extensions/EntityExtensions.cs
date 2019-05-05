using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using DefaultEcs;

using Hazmat.Components;

namespace Hazmat.Utilities.Extensions
{
    public static class EntityExtensions
    {
        public static void SetModelAnimation(this Entity entity, String name)
        {
            if (entity.Has<ModelAnimationComponent>())
            {
                ref var animation = ref entity.Get<ModelAnimationComponent>();

                animation.animations.SetClip(animation.animations.Clips[name]);
            }

            foreach (var child in entity.GetChildren())
            {
                child.SetModelAnimation(name);
            }
        }

        public static void SyncModelAnimation(this Entity entity)
        {
            ref var animation = ref entity.Get<ModelAnimationComponent>();

            var time = animation.animations.CurrentTime;

            foreach (var child in entity.GetChildren())
            {
                child.SyncModelAnimation(time);
            }
        }

        public static void SyncModelAnimation(this Entity entity, TimeSpan time)
        {
            if (entity.Has<ModelAnimationComponent>())
            {
                ref var animation = ref entity.Get<ModelAnimationComponent>();

                animation.animations.Update(time, false, Matrix.Identity);
            }

            foreach (var child in entity.GetChildren())
            {
                child.SyncModelAnimation(time);
            }
        }

        public static bool Has(this Entity entity, Type type)
        {
            return (bool)typeof(Entity).GetMethod("Has").MakeGenericMethod(type).Invoke(entity, null);
        }

        public static void Delete(this Entity entity)
        {
            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                aabb.quadtree.RemoveNode(aabb.element);
            }

            entity.Dispose();
        }
    }
}

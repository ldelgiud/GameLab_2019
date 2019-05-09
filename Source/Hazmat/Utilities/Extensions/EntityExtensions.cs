using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;
using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.ResourceManagers;

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
            if (!entity.IsAlive) return;

            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                aabb.quadtree.RemoveNode(aabb.element);
            }

            entity.Dispose();
        }


        public static Entity SetAttachment(
            this Entity entity, 
            string modelName, 
            string modelTexture,
            Vector3? position = null,
            Vector3? scale = null,
            Vector3? rotation = null,
            string shaderName = "toon", 
            float lineThickness = 0.5f)
        {
            if (!entity.Has<Transform3DComponent>())
            {
                throw new Exception("Cannot attach an object to an entity that has no Transform3DComponent!");
            } 
            else
            {
     
                ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();

                var childEntity = SpawnHelper.World.CreateEntity();
                childEntity.SetAsChildOf(entity);

                Vector3 position_ = position == null ? new Vector3(0f) : position.Value;
                Vector3 scale_ = scale == null ? new Vector3(0.07f) : scale.Value;
                Vector3 rotation_ = rotation == null ? new Vector3(0, 0, MathHelper.PiOver2) : rotation.Value;

                childEntity.Set(new NameComponent() { name = "attachement" });
                childEntity.Set(new Transform3DComponent(new Transform3D(parent: transform.value, position: position_)));
                childEntity.Set(new WorldSpaceComponent());
                childEntity.Set(new ManagedResource<ModelInfo, ModelAlias>(new ModelInfo(
                    modelName,
                    modelTexture,
                    rotation: rotation_,
                    scale: scale_,
                    standardEffect: Hazmat.Instance.Content.Load<Effect>(@"shaders/" + shaderName),
                    standardEffectInitialize: new Tuple<string, float>[] { new Tuple<string, float>("LineThickness", lineThickness) }
                )));
                return childEntity;
            }
        }

    }
}

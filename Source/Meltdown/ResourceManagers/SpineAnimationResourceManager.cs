using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Spine;

using Meltdown.Components;
using Meltdown.Utilities;

namespace Meltdown.ResourceManagers
{
    class SpineAnimationResourceManager : AResourceManager<SpineAnimationInfo, SpineAnimationAlias>
    {

        XnaTextureLoader loader;

        Dictionary<String, SkeletonData> skeletonCache = new Dictionary<String, SkeletonData>();

        public SpineAnimationResourceManager(GraphicsDevice graphicsDevice)
        {
            this.loader = new XnaTextureLoader(graphicsDevice);
        }

        protected override SpineAnimationAlias Load(SpineAnimationInfo info)
        {
            Skeleton skeleton;
            if (this.skeletonCache.TryGetValue(info.name, out SkeletonData cacheData))
            {
                skeleton = new Skeleton(cacheData);
            }
            else
            {
                var atlas = new Atlas(@"Content\" + info.name + ".atlas", this.loader);
                var skeletonJson = new SkeletonJson(atlas);
                var skeletonData = skeletonJson.ReadSkeletonData(@"Content\" + info.name + ".json");
                this.skeletonCache.Add(info.name, skeletonData);
                skeleton = new Skeleton(skeletonData);
            }
            
            skeleton.SetSkin(info.skeletonInfo.skin);

            AnimationState animationState = null;
            if (info.animationStateInfo.HasValue)
            {
                var animationStateInfo = info.animationStateInfo.Value;
                var animationStateData = new AnimationStateData(skeleton.Data);

                animationState = new AnimationState(animationStateData);
                animationState.SetAnimation(0, animationStateInfo.animation, animationStateInfo.loop);
            }

            return new SpineAnimationAlias(
                    animationState,
                    skeleton
                );
        }

        protected override void OnResourceLoaded(in Entity entity, SpineAnimationInfo info, SpineAnimationAlias resource)
        {
            if (info.skeletonInfo.scale.X < 0 && info.skeletonInfo.scale.Y < 0)
            {
                float[] vertexBuffer = null;
                resource.skeleton.GetBounds(out float x, out float y, out float width, out float height, ref vertexBuffer);

                info.skeletonInfo.scale = new Vector2(info.skeletonInfo.width / width, info.skeletonInfo.height / height);
            }

            entity.Set(new SkeletonComponent(resource.skeleton, info.skeletonInfo));

            if (resource.animationState != null)
            {
                entity.Set(new AnimationStateComponent(resource.animationState));
            }
        }
    }
}

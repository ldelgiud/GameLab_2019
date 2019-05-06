using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.Resource;

using Spine;

using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.ResourceManagers
{
    class SpineAnimationResourceManager : AResourceManager<SpineAnimationInfo, SkeletonDataAlias>
    {

        XnaTextureLoader loader;

        Dictionary<String, SkeletonData> skeletonDataCache = new Dictionary<String, SkeletonData>();
        Dictionary<String, Atlas> atlasCache = new Dictionary<String, Atlas>();

        public SpineAnimationResourceManager(GraphicsDevice graphicsDevice)
        {
            Debug.WriteLine(graphicsDevice.GraphicsDeviceStatus);
            this.loader = new XnaTextureLoader(graphicsDevice);
        }

        public void Load(String name)
        {
            var atlas = new Atlas(@"Content\" + name + ".atlas", this.loader);
            this.atlasCache[name] = atlas;

            var skeletonJson = new SkeletonJson(atlas);
            this.skeletonDataCache[name] = skeletonJson.ReadSkeletonData(@"Content\" + name + ".json");
        }

        protected override SkeletonDataAlias Load(SpineAnimationInfo info)
        {
            if (!this.skeletonDataCache.ContainsKey(info.name))
            {
                this.Load(info.name);
            }

            return new SkeletonDataAlias(
                this.skeletonDataCache[info.name]
                );
        }

        protected override void OnResourceLoaded(in Entity entity, SpineAnimationInfo info, SkeletonDataAlias resource)
        {
            var skeleton = new Skeleton(resource.skeletonData);
            skeleton.SetSkin(info.skeletonInfo.skin);
            
            if (info.skeletonInfo.scale.X < 0 && info.skeletonInfo.scale.Y < 0)
            {
                float[] vertexBuffer = null;
                skeleton.GetBounds(out float x, out float y, out float width, out float height, ref vertexBuffer);

                info.skeletonInfo.scale = new Vector3(info.skeletonInfo.width / width, info.skeletonInfo.height / height, 1);
            }

            AnimationState animationState = null;
            if (info.animationStateInfo.HasValue)
            {
                var animationStateInfo = info.animationStateInfo.Value;

                animationState = new AnimationState(new AnimationStateData(skeleton.data));
                animationState.SetAnimation(0, animationStateInfo.animation, animationStateInfo.loop);
            }

            entity.Set(new SpineSkeletonComponent(skeleton, info.skeletonInfo));

            if (animationState != null)
            {
                entity.Set(new SpineAnimationComponent(animationState));
            }
        }
    }
}

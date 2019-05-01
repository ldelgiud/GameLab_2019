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

        Dictionary<String, SkeletonData> skeletonCache = new Dictionary<String, SkeletonData>();

        public SpineAnimationResourceManager(GraphicsDevice graphicsDevice)
        {
            this.loader = new XnaTextureLoader(graphicsDevice);
        }

        protected override SkeletonDataAlias Load(SpineAnimationInfo info)
        {
                var atlas = new Atlas(@"Content\" + info.name + ".atlas", this.loader);
                var skeletonJson = new SkeletonJson(atlas);
                var skeletonData = skeletonJson.ReadSkeletonData(@"Content\" + info.name + ".json");

            return new SkeletonDataAlias(
                    skeletonData
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

                info.skeletonInfo.scale = new Vector2(info.skeletonInfo.width / width, info.skeletonInfo.height / height);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SpineAnimationResourceManager(GraphicsDevice graphicsDevice)
        {
            this.loader = new XnaTextureLoader(graphicsDevice);
        }

        protected override SpineAnimationAlias Load(SpineAnimationInfo info)
        {
            var atlas = new Atlas(@"Content\" + info.name + ".atlas", this.loader);

            var skeletonJson = new SkeletonJson(atlas);
            var skeletonData = skeletonJson.ReadSkeletonData(@"Content\" + info.name + ".json");

            var skeleton = new Skeleton(skeletonData);

            var animationStateData = new AnimationStateData(skeleton.Data);

            var animationState = new AnimationState(animationStateData);
            animationState.SetAnimation(0, info.animationStateInfo.animation, info.animationStateInfo.loop);

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

            entity.Set(new AnimationStateComponent(resource.animationState));
            entity.Set(new SkeletonComponent(resource.skeleton, info.skeletonInfo));
        }
    }
}

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Spine;

namespace Meltdown.Utilities
{
    struct ModelAlias : IDisposable
    {
        public Model value;

        public void Dispose()
        {

        }
    }

    struct Texture2DAlias : IDisposable
    {
        public Texture2D value;

        public void Dispose()
        {

        }
    }

    struct SpineAnimationAlias : IDisposable
    {
        public AnimationState animationState;
        public Skeleton skeleton;

        public SpineAnimationAlias(AnimationState animationState, Skeleton skeleton)
        {
            this.animationState = animationState;
            this.skeleton = skeleton;
        }


        public void Dispose()
        {

        }

    }
}

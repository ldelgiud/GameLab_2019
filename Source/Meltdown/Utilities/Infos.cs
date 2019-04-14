using System;

using Microsoft.Xna.Framework;

using Spine;

namespace Meltdown.Utilities
{
    struct Texture2DInfo
    {
        public String name;
        public Vector2 translation;
        public float rotation;
        public Vector2 scale;
        public float layer;
        public Rectangle? bounds;
        
        public float width;
        public float height;
        
        public Texture2DInfo(String name, float width, float height, Vector2? translation = null, float? rotation = null, float? layer = null, Rectangle? bounds = null)
        {
            this.name = name ?? "";
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = new Vector2(-1, -1);
            this.layer = layer ?? Constants.LAYER_FOREGROUND;
            this.bounds = bounds;

            this.width = width;
            this.height = height;
        }

        public Texture2DInfo(String name, Vector2? translation = null, float? rotation = null, Vector2? scale = null, float? layer = null, Rectangle? bounds = null)
        {
            this.name = name;
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = scale ?? Vector2.One;
            this.layer = layer ?? Constants.LAYER_FOREGROUND;
            this.bounds = bounds;
            this.width = 0;
            this.height = 0;
        }
    }

    struct ModelInfo
    {
        public String name;
        public String textureName;

        public ModelInfo(String name, String textureName = null)
        {
            this.name = name;
            this.textureName = textureName;
        }
    }

    struct SpineAnimationInfo
    {
        public String name;

        public SkeletonInfo skeletonInfo;
        public AnimationStateInfo? animationStateInfo;

        public SpineAnimationInfo(String name, SkeletonInfo skeletonInfo, AnimationStateInfo? animationStateInfo)
        {
            this.name = name;
            this.skeletonInfo = skeletonInfo;
            this.animationStateInfo = animationStateInfo;
        }
    }

    struct SkeletonInfo
    {
        public String skin;
        public Vector2 translation;
        public Vector2 scale;

        public float width;
        public float height;

        public SkeletonInfo(float width, float height, String skin = "default", Vector2? translation = null)
        {
            this.skin = skin;
            this.translation = translation ?? Vector2.Zero;
            this.scale = -Vector2.One;

            this.width = width;
            this.height = height;
        }

        public SkeletonInfo(String skin = "default", Vector2? translation = null, Vector2? scale = null)
        {
            this.skin = skin;
            this.translation = translation ?? Vector2.Zero;
            this.scale = scale ?? Vector2.One;
            this.width = 0;
            this.height = 0;
        }
    }

    struct AnimationStateInfo
    {
        public String animation;
        public bool loop;

        public AnimationStateInfo(String animation, bool loop)
        {
            this.animation = animation;
            this.loop = loop;
            
        }
    }
}
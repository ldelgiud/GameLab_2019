﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Spine;

namespace Hazmat.Utilities
{
    interface Transform2DInfo
    {
        Vector2 Translation { get; }
        float Rotation { get; }
        Vector2 Scale { get; }
    }


    struct Texture2DInfo : Transform2DInfo
    {
        public Vector2 Translation { get { return this.translation; } }
        public float Rotation { get { return this.rotation; } }
        public Vector2 Scale { get { return this.scale; } }

        public String name;
        public Vector2 translation;
        public float rotation;
        public Vector2 scale;
        public float layer;
        public Rectangle? bounds;

        public Effect standardEffect;
        public Effect tempEffect;

        // For variable that changes over time
        public String standardParameterToUpdate;
        public String tempParameterToUpdate;

        public float width;
        public float height;

        public Texture2DInfo(
            String name, 
            float width, 
            float height, 
            Vector2? translation = null, 
            float? rotation = null, 
            float? layer = null, 
            Rectangle? bounds = null, 
            Effect standardEffect = null,
            Effect tempEffect = null,
            String standardParameterToUpdate = null,
            String tempParameterToUpdate = null
            )
        {
            this.name = name ?? "";
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = new Vector2(-1, -1);
            this.layer = layer ?? Constants.LAYER_FOREGROUND;
            this.bounds = bounds;

            this.standardEffect = standardEffect;
            this.tempEffect = tempEffect;
            this.standardParameterToUpdate = standardParameterToUpdate;
            this.tempParameterToUpdate = tempParameterToUpdate;
            
    
            this.width = width;
            this.height = height;
        }

        public Texture2DInfo(
            String name,
            Vector2? translation = null,
            float? rotation = null, 
            Vector2? scale = null, 
            float? layer = null, 
            Rectangle? bounds = null,
            Effect standardEffect = null,
            Effect tempEffect = null,
            String standardParameterToUpdate = null,
            String tempParameterToUpdate = null
            )
        {
            this.name = name;
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = scale ?? Vector2.One;
            this.layer = layer ?? Constants.LAYER_FOREGROUND;
            this.bounds = bounds;
            this.standardEffect = standardEffect;

            this.standardEffect = standardEffect;
            this.tempEffect = tempEffect;
            this.standardParameterToUpdate = standardParameterToUpdate;
            this.tempParameterToUpdate = tempParameterToUpdate;

            this.width = 0;
            this.height = 0;
        }
    }

    struct TextInfo : Transform2DInfo
    {
        public Vector2 Translation { get { return this.translation; } }
        public float Rotation { get { return this.rotation; } }
        public Vector2 Scale { get { return this.scale; } }

        public String text;
        public String font;
        public Color color;
        public Vector2 translation;
        public float rotation;
        public Vector2 scale;

        public TextInfo(String text, String font, Color? color, Vector2? translation = null, float? rotation = null, Vector2? scale = null)
        {
            this.text = text;
            this.font = font;
            this.color = color ?? Color.Black;
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = scale ?? Vector2.One;
        }
    }

    public struct ModelInfo
    {
        public String name;
        public String textureName;
        public Vector3 rotation;
        public Vector3 scale;
        public Vector3 translation;
        public String animation;
        public Effect standardEffect;
        public bool updateTimeEffect;
        public bool cachedUpdateTimeEffect;

        public ModelInfo(String name, String textureName = null, Vector3? translation = null, Vector3? rotation = null, Vector3? scale = null, String animation = null, Effect standardEffect = null, bool? updateTimeEffect = null, IEnumerable<Tuple<string, float>> standardEffectInitialize = null)
        {
            this.name = name;
            this.textureName = textureName;
            this.translation = translation ?? Vector3.Zero;
            this.rotation = rotation ?? Vector3.Zero;
            this.scale = scale ?? Vector3.One;
            this.animation = animation;
            this.standardEffect = standardEffect ?? null;
            this.updateTimeEffect = updateTimeEffect ?? false;
            this.cachedUpdateTimeEffect = false;

            if (standardEffectInitialize != null)
            {
                foreach (Tuple<string, float> p in standardEffectInitialize)
                {
                    standardEffect.Parameters[p.Item1].SetValue(p.Item2);
                }

            }
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
        public Vector3 translation;
        public Vector3 scale;

        public float width;
        public float height;

        public SkeletonInfo(float width, float height, String skin = "default", Vector3? translation = null)
        {
            this.skin = skin;
            this.translation = translation ?? Vector3.Zero;
            this.scale = -Vector3.One;

            this.width = width;
            this.height = height;
        }

        public SkeletonInfo(String skin = "default", Vector3? translation = null, Vector3? scale = null)
        {
            this.skin = skin;
            this.translation = translation ?? Vector3.Zero;
            this.scale = scale ?? Vector3.One;
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

    struct TileModelInfo
    {
        public String name;

        public TileModelInfo(String name)
        {
            this.name = name;
        }
    }
}

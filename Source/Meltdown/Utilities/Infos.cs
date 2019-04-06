using System;

using Microsoft.Xna.Framework;

namespace Meltdown.Utilities
{
    struct Texture2DInfo
    {
        public String name;
        public Vector2 translation;
        public float rotation;
        public Vector2 scale;
        
        public float width;
        public float height;
        
        public Texture2DInfo(String name, float width, float height, Vector2? translation = null, float? rotation = null)
        {
            this.name = name ?? "";
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = new Vector2(-1, -1);
            this.width = width;
            this.height = height;
        }

        public Texture2DInfo(String name, Vector2? translation = null, float? rotation = null, Vector2? scale = null)
        {
            this.name = name;
            this.translation = translation ?? Vector2.Zero;
            this.rotation = rotation ?? 0;
            this.scale = scale ?? Vector2.One;
            this.width = 0;
            this.height = 0;
        }
    }

    struct ModelInfo
    {
        public String name;
        public Vector3 translation;
        public Vector3 rotation;
        public Vector3 scale;

        public ModelInfo(String name, Vector3? translation = null, Vector3? rotation = null, Vector3? scale = null)
        {
            this.name = name;
            this.translation = translation ?? Vector3.Zero;
            this.rotation = rotation ?? Vector3.Zero;
            this.scale = scale ?? Vector3.One;
        }
    }
}
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
        
        public Texture2DInfo(String name, Vector2? translation, float? rotation, float width, float height)
        {
            this.name = name;
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
}
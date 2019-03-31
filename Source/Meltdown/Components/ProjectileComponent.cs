using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Components
{
    public class ProjectileComponent
    {
        public Texture2D projTex;
        public float speed;
        public float radiusRange;
        public float damage;

        public ProjectileComponent(float damage, float speed, float radiusRange, Texture2D projTex)
        {
            this.speed = speed;
            this.damage = damage;
            this.radiusRange = radiusRange;
            this.projTex = projTex;
        }

    }
}

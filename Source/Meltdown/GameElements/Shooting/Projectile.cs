using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.GameElements.Shooting
{
    public class Projectile
    {
        public Texture2D projTex;
        public float speed;
        public float radiusRange;

        public Projectile(float speed, float radiusRange, Texture2D projTex)
        {
            this.speed = speed;
            this.radiusRange = radiusRange;
            this.projTex = projTex;
        }

    }
}

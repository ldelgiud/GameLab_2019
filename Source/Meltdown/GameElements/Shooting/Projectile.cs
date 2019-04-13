using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.GameElements.Shooting
{
    public class Projectile
    {
        public float speed;
        public float radiusRange;

        public ProjectileComponent(float damage, float speed, float radiusRange)
        {
            this.speed = speed;
            this.radiusRange = radiusRange;
        }

    }
}

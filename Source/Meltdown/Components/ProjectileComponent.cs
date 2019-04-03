using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Components
{
    public class ProjectileComponent
    {
        public float speed;
        public float radiusRange;
        public float damage;

        public ProjectileComponent(float damage, float speed, float radiusRange)
        {
            this.speed = speed;
            this.damage = damage;
            this.radiusRange = radiusRange;
        }

    }
}

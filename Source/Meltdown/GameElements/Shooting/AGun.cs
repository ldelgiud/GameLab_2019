using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Components;

using DefaultEcs;

namespace Meltdown.GameElements.Shooting
{
    abstract class AGun
    {
        public ProjectileComponent projectile;
        public float reloadTime;
        public float timeLastShot;

        //public abstract void Shoot(float absoluteValue, Vector2 position, Vector2 direction, World world);
        public abstract void Shoot(float absoluteValue, WorldTransformComponent transform, Vector2 direction, World world);

    }
}

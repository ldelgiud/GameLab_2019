using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hazmat.Graphics;
using Hazmat.Components;

using DefaultEcs;

namespace Hazmat.GameElements.Shooting
{
    abstract class AGun
    {
        public float damage;
        public float additionalDamage = 0; // in case of upgrades
        public float projectileSpeed;
        public float radiusRange;
        public float reloadTime;
        public string projectileTexture;
        public float timeLastShot;
        public Alliance alliance;

        public abstract void Shoot(float absoluteValue, Transform3D transform, Vector2 direction);

    }
}

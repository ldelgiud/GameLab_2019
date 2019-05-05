using System;

namespace Hazmat.Components
{
    public class StatsComponent
    {

        public float Speed;
        public float Damage;

        public StatsComponent(float speed, float damage)
        {
            this.Speed = speed;
            this.Damage = damage;
        }

        /* For testing now.
         * Later add some sort of upgrade level (cannot have more than a maximal speed).
         * When max level is reached, this powerup cannot be shown for speed anymore.
        */
        public void UpgradeSpeed(int amount = 2)
        {
            this.Speed += amount;
        }

        public void UpgradeDamage(int amount = 10)
        {
            this.Damage += amount;
        }
    }
}

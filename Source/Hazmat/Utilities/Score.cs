using System;

namespace Hazmat.Utilities
{
    class Score
    {
        public float TimeStart;
        public float TimeEnd;
        public int Kills = 0;
        public int Batteries = 0;
        public int ArmorUpgrades = 0;
        public int WeaponUpgrades = 0;

        public Score(Time time)
        {
            this.TimeStart = time.Absolute;
        }

        public void Complete(Time time)
        {
            this.TimeEnd = time.Absolute;
        }

    }
}
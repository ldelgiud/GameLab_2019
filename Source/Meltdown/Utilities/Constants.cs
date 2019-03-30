using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Utilities
{
    static class Constants
    {
        public static double PLANT_PLAYER_DISTANCE = 500.0;
        public static Random RANDOM = new Random();
        public static uint SMALL_BATTERY_SIZE = 25;
        public static uint MEDIUM_BATTERY_SIZE = 50;
        public static uint BIG_BATTERY_SIZE = 100;
        public static float DRONE_SPEED = 4;
        public static float SHOOTER_SPEED = 3;

        //AI values for FSM transitions
        public static double STANDBY_TO_SEARCH_DIST = 80;
        public static double SEARCH_TO_STANDBY_DIST = 90;
        public static double SEARCH_TO_ATTACK_DIST = 40;
        public static double ATTACK_TO_SEARCH_DIST = 50;
        //Maximum energy for players
        public static double MAX_ENERGY = 1000;
        //debug value to stop generating enemies in EnemySpawnSystem
        public static uint MAX_AMOUNT_OF_ENEMIES = 15;

        // size of tiles for procedural generation
        public static float TILE_SIZE = 100;
        public static float LEFT_BORDER = -300;
        public static float RIGHT_BORDER = 2000;
        public static float TOP_BORDER = 2000;
        public static float BOTTOM_BORDER = -300;



    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.Utilities
{
    static class Constants
    {

        public static double PLANT_PLAYER_DISTANCE = 500.0;
        public static Random RANDOM = new Random();

        //BATTERY SIZES
        public static uint SMALL_BATTERY_SIZE = 25;
        public static uint MEDIUM_BATTERY_SIZE = 50;
        public static uint BIG_BATTERY_SIZE = 100;

        //ENTITY PROPERTIES
        public static float DRONE_SPEED = 4;
        public static float SHOOTER_SPEED = 3;
        public static int PLAYER_SPEED = 40;
        public static float TTL_BULLET = 5f;

        //GRID GENERATION VALUES
            public static float NODE_RADIUS = 0.5f;
            public static float NODE_HIT_RADIUS = 3f;
        //AI values for FSM transitions
            public static double OFFLINE_TO_STANDBY_DIST = 70;
            public static double STANDBY_TO_OFFLINE_DIST = 80;
            public static double STANDBY_TO_SEARCH_DIST = 50;
            public static double SEARCH_TO_STANDBY_DIST = 60;
            public static double BLIND_STANDBY_TO_SEARCH_DIST = 30;
            public static double SEARCH_TO_ATTACK_DIST = 25;
            public static double ATTACK_TO_SEARCH_DIST = 30;
        //SQUARED VERSIONS OF PREV NUMBERS 
            public static double OFFLINE_TO_STANDBY_SQRD_DIST = OFFLINE_TO_STANDBY_DIST * OFFLINE_TO_STANDBY_DIST;
            public static double STANDBY_TO_OFFLINE_SQRD_DIST = STANDBY_TO_OFFLINE_DIST * STANDBY_TO_OFFLINE_DIST;
            public static double STANDBY_TO_SEARCH_SQRD_DIST = STANDBY_TO_SEARCH_DIST * STANDBY_TO_SEARCH_DIST;
            public static double BLIND_STANDBY_TO_SEARCH_SQRD_DIST = BLIND_STANDBY_TO_SEARCH_DIST * BLIND_STANDBY_TO_SEARCH_DIST;
            public static double SEARCH_TO_STANDBY_SQRD_DIST = SEARCH_TO_STANDBY_DIST * SEARCH_TO_STANDBY_DIST;
            public static double SEARCH_TO_ATTACK_SQRD_DIST = SEARCH_TO_ATTACK_DIST * SEARCH_TO_ATTACK_DIST;
            public static double ATTACK_TO_SEARCH_SQRD_DIST = ATTACK_TO_SEARCH_DIST * ATTACK_TO_SEARCH_DIST;
        //Maximum energy for players
            public static double MAX_ENERGY = 1000;
        //debug value to stop generating enemies in EnemySpawnSystem
        public static uint MAX_AMOUNT_OF_ENEMIES = 15;
        public static uint INTERACTION_DISTANCE = 7;

        // size of tiles for procedural generation
        public static float TILE_SIZE = 10;
        public static float LEFT_BORDER = -300;
        public static float RIGHT_BORDER = 2000;
        public static float TOP_BORDER = 2000;
        public static float BOTTOM_BORDER = -300;
        public static Vector2 TOP_RIGHT_CORNER =
            new Vector2(Constants.RIGHT_BORDER, Constants.TOP_BORDER);
        public static Vector2 BOTTOM_LEFT_CORNER =
            new Vector2(Constants.LEFT_BORDER, Constants.BOTTOM_BORDER);

        //
        public static float HOLD_THRESHOLD = 0.25f;

        // Layer values
        public const float LAYER_FOREGROUND = 0.01f;
        public const float LAYER_BACKGROUND = 0.0f;
        public const float LAYER_BACKGROUND_DETAIL = 0.001f;
        public const float LAYER_BACKGROUND_DEBUG = 0.0011f;
    }
}

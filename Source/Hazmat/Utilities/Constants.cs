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
        //RANDOM NUMBER GENERATOR USED THROUGHOUT THE GAME
        public static Random RANDOM = new Random();

        //PLAYER STATS
        public static float PLAYER_RELOAD_TIME = 0.3f;              //TESTED
        public static float BULLET_SIZE = 1f;                       //TESTED
        public static int PLAYER_INITIAL_SPEED = 20; //TESTED
        public static double PLAYER_INITIAL_ENERGY = 1000;
        //PLAYER UPGRADES
        public static int SPEED_UPGRADE = 3;
        public static int DAMAGE_UPGRADE = 30;

        //PROCGEN
        public static double STREET_SPAWN_RATE = 0.6;
        public static float TILE_SIZE = 10;
        public static float LEFT_BORDER = -200;
        public static float RIGHT_BORDER = 800;
        public static float TOP_BORDER = 800;
        public static float BOTTOM_BORDER = -200;
        public static Vector2 TOP_RIGHT_CORNER =
            new Vector2(Constants.RIGHT_BORDER, Constants.TOP_BORDER);
        public static Vector2 BOTTOM_LEFT_CORNER =
            new Vector2(Constants.LEFT_BORDER, Constants.BOTTOM_BORDER);

        //POWERPLANT STATS
        public static double PLANT_PLAYER_DISTANCE = 750;
        public static double MIN_DEGREE = 20;
        public static double MAX_DEGREE = 70;
        public static double MIN_RADIAN = Constants.MIN_DEGREE * (Math.PI / 180);
        public static double MAX_RADIAN = Constants.MAX_DEGREE * (Math.PI / 180);

        //BATTERIES
        public static Vector2 BIG_BATTERY_SCALE = new Vector2(4f, 4f);
        public static uint SMALL_BATTERY_SIZE = 250;
        public static uint MEDIUM_BATTERY_SIZE = 500;
        public static uint BIG_BATTERY_SIZE = 1000;

        //DRONES
        public static float MEMORY_OF_HIT = 5f;
        public static float DRONE_SPEED = 23; //TESTED

        //SHOOTERS
        public static float SHOOTER_SPEED = 20;//20;

        //MAILBOXES
        public static float MAILBOX_DAMAGE = 100;
        public static float MAILBOX_RELOAD_TIME = 1f;

        //SHOOTING
        public static float TTL_BULLET = 5f; //TESTED
        public static float BULLET_SPEED = 45; // TESTED

        //GRID GENERATION VALUES
        public static float NODE_RADIUS = 1f;
        public static float NODE_HIT_RADIUS = 3f;

        //AI values for FSM transitions
        public static double OFFLINE_TO_STANDBY_DIST = 60;
        public static double OFFLINE_TO_ATTACK_DIST = 35;       //ONLY FOR MAILBOX!
        public static double STANDBY_TO_OFFLINE_DIST = 70;
        public static double STANDBY_TO_SEARCH_DIST = 40;
        public static double SEARCH_TO_STANDBY_DIST = 50;
        public static double BLIND_STANDBY_TO_SEARCH_DIST = 20;
        public static double SEARCH_TO_ATTACK_DIST = 35;        //ONLY FOR SHOOTER
        public static double ATTACK_TO_SEARCH_DIST = 40;        //ONLY FOR SHOOTER
        public static double ATTACK_TO_OFFLINE_DIST = 30;       //ONLY FOR MAILBOX!
        public static double DIRECT_ATTACK_DIST = 4; 
        //SQUARED VERSIONS OF PREV NUMBERS 
        public static double OFFLINE_TO_STANDBY_SQRD_DIST = OFFLINE_TO_STANDBY_DIST * OFFLINE_TO_STANDBY_DIST;
        public static double OFFLINE_TO_ATTACK_SQRD_DIST = OFFLINE_TO_ATTACK_DIST * OFFLINE_TO_ATTACK_DIST;
        public static double STANDBY_TO_OFFLINE_SQRD_DIST = STANDBY_TO_OFFLINE_DIST * STANDBY_TO_OFFLINE_DIST;
        public static double STANDBY_TO_SEARCH_SQRD_DIST = STANDBY_TO_SEARCH_DIST * STANDBY_TO_SEARCH_DIST;
        public static double BLIND_STANDBY_TO_SEARCH_SQRD_DIST = BLIND_STANDBY_TO_SEARCH_DIST * BLIND_STANDBY_TO_SEARCH_DIST;
        public static double SEARCH_TO_STANDBY_SQRD_DIST = SEARCH_TO_STANDBY_DIST * SEARCH_TO_STANDBY_DIST;
        public static double SEARCH_TO_ATTACK_SQRD_DIST = SEARCH_TO_ATTACK_DIST * SEARCH_TO_ATTACK_DIST;
        public static double ATTACK_TO_SEARCH_SQRD_DIST = ATTACK_TO_SEARCH_DIST * ATTACK_TO_SEARCH_DIST;
        public static double ATTACK_TO_OFFLINE_SQRD_DIST = ATTACK_TO_OFFLINE_DIST * ATTACK_TO_OFFLINE_DIST;
        public static double DIRECT_ATTACK_SQRD_DIST = DIRECT_ATTACK_DIST * DIRECT_ATTACK_DIST;

        //INTERACTABLE OBJECTS
        public static uint INTERACTION_DISTANCE = 2;

        //Input
        public static float HOLD_THRESHOLD = 0.25f;

        // PowerUp Values
        public const float POWERUP_DISPLAY_TIME = 5f; // almost 5 to avoid seeing the red line before ending

        // Layer values
        public const float LAYER_FOREGROUND = 0.01f;
        public const float LAYER_BACKGROUND = 0.0f;
        public const float LAYER_BACKGROUND_DETAIL = 0.001f;
        public const float LAYER_BACKGROUND_DEBUG = 0.0011f;


        //Names
        public const string STREET_TILE_NAME = "streetTile";
        public static string PLAYER_NAME = "player_0";
        public static string GUN_NAME = "gun";
        public static string POWERPLANT_NAME = "powerplant";
        public static string HOUSE_0_NAME = "house_0";
        public static string HOUSE_1_NAME = "house_1";
        public static string SIDEWALK_NAME = "sidewalk";
        public static string LAMP_NAME = "lamp";
        public static string ROCK_NAME = "rock";
        public static string BATTERY_NAME = "battery_0";
        public static string DRONE_NAME = "drony";
        public static string MAILBOX_NAME = "mailbox";
        public static string SHOOTER_NAME = "shooter";
        public static string POWERUP_NAME = "power_up";
        public static string LOOTING_STATION_NAME = "loot_station";
        public static string WALL_NAME = "wall";
        public static string ROADBLOCK_NAME = "roadblock";
    }
}

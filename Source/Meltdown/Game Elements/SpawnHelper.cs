using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Components;
using Meltdown.AI;

using Nez;
using Nez.Sprites;

namespace Meltdown.Game_Elements
{
    class SpawnHelper
    {

        public static ContentManager Content { get
            {
                return Game1.Instance.Content;
            }
        }

        
        
        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static PlayerInfo SpawnPLayer(
            Scene scene,
            int playerID)
        {
            //Generate position
            Vector2 position = new Vector2(0, 0);
            var texture = scene.content.Load<Texture2D>("player1 PLACEHOLDER");
            //Create entity and attach the components to it
            var entity = scene.createEntity("player" + playerID);
            entity.transform.position = position;
            entity.addComponent(new VelocityComponent(new Vector2(0, 0)));
            entity.addComponent(new Sprite(texture));
            entity.addComponent(new PlayerComponent(playerID));
            entity.addComponent(new BoxCollider());
            return new PlayerInfo(new Vector2(0, 0));
        }

        /// <summary>
        /// Spawn Nuclear Power Plant with all entities and attach respective components
        /// </summary>
        /// <param name="plant">Powerlplant object</param>
        public static void SpawnNuclearPowerPlant(
            PowerPlant plant,
            Scene scene)
        {
            //Generate random position
            double angle = Constants.RANDOM.NextDouble() * Math.PI / 2.0;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            plant.Position = position;

            var texture = scene.content.Load<Texture2D>("NuclearPlantPLACEHOLDER");


            //Create entity and attach the components to it
            var entity = scene.createEntity("powerPlant");
            entity.addComponent(new Sprite(texture));
            entity.transform.position = position;
            entity.addComponent(new BoxCollider());

        }

        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="energy">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        public static void SpawnBattery(int energy, 
            Vector2 position,
            Scene scene)
        {

            var texture = scene.content.Load<Texture2D>("battery PLACEHOLDER");
            
            //Create entity and attach its components
            var entity = scene.createEntity("battery");
            entity.addComponent(new BatteryComponent(energy));
            entity.transform.position = position;
            entity.addComponent(new Sprite(texture));
            entity.addComponent(new CircleCollider());
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(Vector2 position, Scene scene)
        {
            var texture = scene.content.Load<Texture2D>("EnemyPLACEHOLDER");
        
            //Create entity and attach its components
            var entity = scene.createEntity("enemy");
            entity.transform.position = position;
            entity.addComponent(new VelocityComponent(new Vector2(0, 0)));
            entity.addComponent(new Sprite(texture));
            entity.addComponent(new AIComponent(new StandBy()));
            entity.addComponent(new BoxCollider());
        }
    }
}
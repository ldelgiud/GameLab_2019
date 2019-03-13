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
using MonoGame.Extended.Entities;
using MonoGame.Extended;

namespace Meltdown.Game_Elements
{
    class SpawnHelper
    {
        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="world">World to generate player in</param>
        /// <param name="Content"></param>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static PlayerInfo SpawnPLayer(
            World world, 
            ContentManager Content, 
            int playerID)
        {
            var entity = world.CreateEntity();
            Vector2 DL = new Vector2(0, 0);
            Vector2 TR = new Vector2(100, 100);
            entity.Attach(new PositionComponent(new Vector2(0, 0)));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("player1 PLACEHOLDER")));
            entity.Attach(new PlayerComponent(playerID));
            entity.Attach(new BoundingBoxComponent(new RectangleF(DL, TR)));
            return new PlayerInfo(new Vector2(0,0));

        }

        public static void SpawnNuclearPowerPlant(
            World world, 
            ContentManager Content, 
            PowerPlant plant)
        {
            var entity = world.CreateEntity();
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("NuclearPlantPLACEHOLDER")));

            
            double angle = Constants.RANDOM.NextDouble() * Math.PI / 2.0;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
           
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            entity.Attach(new PositionComponent(position));
            plant.Position = position;
        }


        public static void SpawnBattery(
            World world, 
            ContentManager Content, 
            int size, 
            Vector2 position)
        {
            var entity = world.CreateEntity();
            entity.Attach(new BatteryComponent(size));
            entity.Attach(new PositionComponent(position));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("battery PLACEHOLDER")));
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="world"></param>
        /// <param name="Content"></param>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(
            World world, 
            ContentManager Content, 
            Vector2 pos)
        {
            var entity = world.CreateEntity();
            entity.Attach(new PositionComponent(pos));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("EnemyPLACEHOLDER")));
            entity.Attach(new AIComponent(new StandBy()));
        }

        
    }
}

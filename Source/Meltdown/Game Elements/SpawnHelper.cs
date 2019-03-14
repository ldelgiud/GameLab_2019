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
using Meltdown.States;
using MonoGame.Extended.Collisions;

namespace Meltdown.Game_Elements
{
    class SpawnHelper
    {
        public static World World { get
            {
                return ((GameState)Game1.Instance.ActiveState).world;
            }
        } 

        public static ContentManager Content { get
            {
                return Game1.Instance.Content;
            }
        }

        public static Quadtree quadtree { get
            {
                return ((GameState)Game1.Instance.ActiveState).quadtree;
            }
        }
        
        /// <summary>
        /// Helper function, spawns player at position (0,0) with zero velocity
        /// </summary>
        /// <param name="playerID">starts at 0, and linearly increase, NO RANDOM VARIABLES</param>
        public static PlayerInfo SpawnPLayer(
            int playerID)
        {
            
            var entity = SpawnHelper.World.CreateEntity();
            Vector2 position = new Vector2(0, 0);
            Vector2 TR = new Vector2(100, 100);
            BoundingBoxComponent AABB = new BoundingBoxComponent(new RectangleF(position, TR));
            entity.Attach(new PositionComponent(position));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("player1 PLACEHOLDER")));
            entity.Attach(new PlayerComponent(playerID));
            entity.Attach(AABB);
            SpawnHelper.quadtree.Insert(new QuadtreeData(AABB));
            return new PlayerInfo(new Vector2(0,0));

        }

        public static void SpawnNuclearPowerPlant(PowerPlant plant)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("NuclearPlantPLACEHOLDER")));

            
            double angle = Constants.RANDOM.NextDouble() * Math.PI / 2.0;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
           
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            entity.Attach(new PositionComponent(position));
            plant.Position = position;
        }


        public static void SpawnBattery(int size, 
            Vector2 position)
        {
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new BatteryComponent(size));
            entity.Attach(new PositionComponent(position));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("battery PLACEHOLDER")));
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(Vector2 pos)
        {
            Vector2 TR = new Vector2(100, 100);
            BoundingBoxComponent AABB = new BoundingBoxComponent(new RectangleF(pos, TR));
            var entity = SpawnHelper.World.CreateEntity();

            entity.Attach(new PositionComponent(pos));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("EnemyPLACEHOLDER")));
            entity.Attach(new AIComponent(new StandBy()));
            quadtree.Insert(new QuadtreeData(AABB));
        }

        
    }
}

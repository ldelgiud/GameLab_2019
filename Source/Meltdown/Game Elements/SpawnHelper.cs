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

        public static Quadtree Quadtree { get
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
            //Generate position and bounding box and add to Quadtree
            Vector2 position = new Vector2(0, 0);
            Size2 size = new Size2(250, 280);
            BoundingBoxComponent AABB = new BoundingBoxComponent(new RectangleF(position, size));
            SpawnHelper.Quadtree.Insert(new QuadtreeData(AABB));

            //Create entity and attach the components to it
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new PositionComponent(position));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("player1 PLACEHOLDER")));
            entity.Attach(new PlayerComponent(playerID));
            entity.Attach(AABB);
            return new PlayerInfo(new Vector2(0, 0));
        }

        /// <summary>
        /// Spawn Nuclear Power Plant with all entities and attach respective components
        /// </summary>
        /// <param name="plant">Powerlplant object</param>
        public static void SpawnNuclearPowerPlant(PowerPlant plant)
        {
            //Generate random position
            double angle = Constants.RANDOM.NextDouble() * Math.PI / 2.0;
            double x = Constants.PLANT_PLAYER_DISTANCE * Math.Cos(angle);
            //TODO: change this once camera work is done
            double y = Constants.PLANT_PLAYER_DISTANCE * Math.Sin(angle);
            Vector2 position = new Vector2((float)x, (float)y);
            plant.Position = position;

            //Generate Bounding box and insert into quadtree
            Size2 size = new Size2(200, 200);
            BoundingBoxComponent AABB = new BoundingBoxComponent(new RectangleF(position, size));
            SpawnHelper.Quadtree.Insert(new QuadtreeData(AABB));

            //Create entity and attach the components to it
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("NuclearPlantPLACEHOLDER")));
            entity.Attach(new PositionComponent(position));
            entity.Attach(AABB);

        }

        /// <summary>
        /// Spawns a battery entity with given position and size
        /// </summary>
        /// <param name="energy">Amount of regenrated life 
        /// Please use the sizes given from Constants</param>
        /// <param name="position">position to which battery will spawn</param>
        public static void SpawnBattery(int energy, 
            Vector2 position)
        {
            //Generate Bounding box and add to quadtree
            BoundingBoxComponent AABB = new BoundingBoxComponent(new CircleF(position, 50f));
            SpawnHelper.Quadtree.Insert(new QuadtreeData(AABB));

            //Create entity and attach its components
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new BatteryComponent(energy));
            entity.Attach(new PositionComponent(position));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("battery PLACEHOLDER")));
            entity.Attach(AABB);
        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(Vector2 pos)
        {
            //Generate size and bounding box and add to Quadtree
            Size2 size = new Size2(50, 50);
            BoundingBoxComponent AABB = new BoundingBoxComponent(new RectangleF(pos, size));
            Quadtree.Insert(new QuadtreeData(AABB));

            //Create entity and attach its components
            var entity = SpawnHelper.World.CreateEntity();
            entity.Attach(new PositionComponent(pos));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(
                SpawnHelper.Content.Load<Texture2D>("EnemyPLACEHOLDER")));
            entity.Attach(new AIComponent(new StandBy()));
            entity.Attach(AABB);
        }
    }
}

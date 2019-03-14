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
        public static PlayerInfo SpawnPLayer(World world, ContentManager Content, int playerID)
        {
            var entity = world.CreateEntity();
            entity.Attach(new PositionComponent(new Vector2(0, 0)));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("player")));
            entity.Attach(new PlayerComponent(playerID));
            return new PlayerInfo(new Vector2(0,0));

        }

        /// <summary>
        /// Spawn an enemy entity at given position in standby
        /// </summary>
        /// <param name="world"></param>
        /// <param name="Content"></param>
        /// <param name="pos">Position to Spawn enemy at</param>
        /// <param name="life">Starting Life of Enemy</param>
        public static void SpawEnemy(World world, ContentManager Content, Vector2 pos, float life)
        {
            var entity = world.CreateEntity();
            entity.Attach(new PositionComponent(pos));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("EnemyPLACEHOLDER")));
            entity.Attach(new AIComponent(new StandBy()));
            entity.Attach(new EnemyLifeComponent(life));
        }
    }
}

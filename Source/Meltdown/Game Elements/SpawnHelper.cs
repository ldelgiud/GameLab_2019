using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        public static void SpawnPLayer(World world, ContentManager Content)
        {
            var entity = world.CreateEntity();
            entity.Attach(new PositionComponent(new Vector2(0, 0)));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("player")));
            entity.Attach(new PlayerComponent());

        }

        /// <summary>
        /// Spawn an enemy entity at given position
        /// </summary>
        /// <param name="world"></param>
        /// <param name="Content"></param>
        /// <param name="pos">Position to Spawn enemy at</param>
        public static void SpawEnemy(World world, ContentManager Content, Vector2 pos)
        {
            var entity = world.CreateEntity();
            entity.Attach(new PositionComponent(pos));
            entity.Attach(new VelocityComponent(new Vector2(0, 0)));
            entity.Attach(new TextureComponent(Content.Load<Texture2D>("EnemyPLACEHOLDER")));
        }

        
    }
}

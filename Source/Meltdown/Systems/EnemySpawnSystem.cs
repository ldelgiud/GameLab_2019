using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Systems
{
    class EnemySpawnSystem : ISystem<Time>, IDisposable
    {
        public bool IsEnabled { get; set; } = true;
        EntitySet players;

        public EnemySpawnSystem(World world)
        {
            this.players = world.GetEntities().With<PlayerComponent>().Build();

        }
        public void Update(Time state)
        {
            foreach (Entity entity in this.players.GetEntities())
            {
                bool generate = Constants.RANDOM.NextDouble() < 0.1 * state.Delta;
                if (generate)
                {
                    bool drone = Constants.RANDOM.Next(2) == 1;
                    SpawnHelper.SpawnRandomEnemy(drone, 
                        entity.Get<WorldTransformComponent>().value.position.ToVector2());
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

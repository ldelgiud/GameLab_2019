using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Systems
{
    class EnemySpawnSystem : ISystem<Time>, IDisposable
    {
        public bool IsEnabled { get; set; } = true;
        uint enemyCount;

        public EnemySpawnSystem()
        {
            enemyCount = 0;
        }
        public void Update(Time state)
        {
            if (enemyCount < Constants.MAX_AMOUNT_OF_ENEMIES)
            {
                bool generate = Constants.RANDOM.NextDouble() < 0.4*state.Delta;
                if (generate)
                {
                    bool drone = Constants.RANDOM.Next(2) == 1;
                    SpawnHelper.SpawnRandomEnemy(drone);
                    enemyCount++;
                }
            }
            

        }

        public void Dispose()
        {

        }
    }
}

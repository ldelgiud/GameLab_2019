using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.AI
{
    class DroneStandby : AIState
    {

        const double distToSearch = 600;


        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity)
        {

            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.value.position.ToVector2() - pos;

                if (dist.Length() <= distToSearch) return new DroneSearch();
            }
            return this;
        }
    }
}

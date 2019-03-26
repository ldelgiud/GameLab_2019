using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;

namespace Meltdown.AI
{
    class StandbyState : AIState
    {

        const double distToSearch = 600;


        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity)
        {

            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Position - pos;

                if (dist.Length() <= StandbyState.distToSearch) return new DroneSearch();
            }
            return this;
        }
    }
}

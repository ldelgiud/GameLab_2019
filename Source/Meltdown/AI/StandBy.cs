using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;

namespace Meltdown.AI
{
    class StandBy : AIState
    {
        
        const double distToSearch = 200;
        const double distToAttack = 100;
        

        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, Vector2 velocity)
        {
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.position - pos;

                if (dist.Length() <= StandBy.distToAttack) return new Attack();
                if (dist.Length() <= StandBy.distToSearch) return new Search();
            }

            return this;
        }
    }
}

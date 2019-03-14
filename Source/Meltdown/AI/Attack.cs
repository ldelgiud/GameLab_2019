using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;

namespace Meltdown.AI
{
    class Attack : AIState
    {

        const double distToStanby = 650;
        const double distToSearch = 250;

        
        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: remeber that default closest player is at (0,0)
            PlayerInfo closestPlayer = new PlayerInfo(new Vector2(0,0));
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.position - pos;
                if (dist.Length() < minDist) closestPlayer = player;
                
            }
            Vector2 distVector = closestPlayer.position - pos;
            //ATTACK!
            //TODO: implement attack



            //UPDATE STATE
            if (distVector.Length() >= Attack.distToSearch) return new Search();
            if (distVector.Length() >= Attack.distToStanby) return new StandBy();
            return this;
        }
    }
}

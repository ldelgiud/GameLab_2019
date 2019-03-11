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

        const double distToStanby = 250;
        const double distToSearch = 150;

        
        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            PlayerInfo closestPlayer = new PlayerInfo();
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.position - pos;
                if (dist.Length() < minDist) closestPlayer = player;
                
            }
            Vector2 distVector = closestPlayer.position - pos;

            //ATTACK!
            //TODO: implement attack



            //UPDATE STATE
            //TODO: Should nullcheck closestPlayer.position

            if (distVector.Length() >= Attack.distToSearch) return new Attack();
            if (distVector.Length() >= Attack.distToStanby) return new StandBy();
            return this;
        }
    }
}

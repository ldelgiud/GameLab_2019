using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;

namespace Meltdown.AI
{
    class Search : AIState
    {
        const double distToStanby = 650;
        const double distToAttack = 200;
        const float speed2Norm = 20;



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos, 
            Vector2 pos, 
            ref Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            //default closest player at (0,0)
            PlayerInfo closestPlayer = new PlayerInfo(new Vector2(0, 0));
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.position - pos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = closestPlayer.position - pos;
            double distance = distVector.Length();
            //SEARCH
            distVector.Normalize();
            velocity = Vector2.Multiply(distVector, speed2Norm);
            //TODO: Implement pathfinding method

            //UPDATE STATE
            if (distance <= Search.distToAttack)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new Attack();
            }
            if (distance >= Search.distToStanby)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new StandBy();
            } 
            return this;
        }
    }
}

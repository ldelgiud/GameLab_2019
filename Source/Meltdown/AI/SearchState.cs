using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;

namespace Meltdown.AI
{
    class SearchState : AIState
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
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Position - pos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = closestPlayer.transform.Position - pos;
            double distance = distVector.Length();
            //SEARCH
            distVector.Normalize();
            velocity = Vector2.Multiply(distVector, speed2Norm);
            //TODO: Implement pathfinding method

            //UPDATE STATE
            if (distance <= SearchState.distToAttack)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new AttackState();
            }
            if (distance >= SearchState.distToStanby)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new StandbyState();
            }
            return this;
        }
    }
}

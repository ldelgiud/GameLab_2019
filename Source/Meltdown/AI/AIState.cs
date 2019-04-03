using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;

using Meltdown.Utilities;

namespace Meltdown.AI
{
    abstract class AIState
    {
        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Entity entity, Time time);


        /// <summary>
        /// currently only returns vector pointing towards given player
        /// will later implement pathfinding    
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="myPos"></param>
        /// <returns></returns>
        protected Vector2 Pathfinder(Vector2 playerPos, Vector2 myPos)
        {

            return  playerPos - myPos;
        }

    }


}

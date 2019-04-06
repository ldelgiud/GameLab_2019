using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;

using Meltdown.Utilities;
using Meltdown.Pathfinding;
using System.Collections;

namespace Meltdown.AI
{
    abstract class AIState
    {
        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Entity entity, Time time);
        PathRequestManager pathRequestManager;
        Vector2 position;
        Path path;
        int turnDist = 2;
        const float sqrdUpdateThreshold = 5;
        const float minPathUpdateTime = .2f;
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

        protected void RequestPath(Vector2 start, Vector2 end)
        {
            this.pathRequestManager.RequestPath(start, end, OnPathFound);
        }

        public void OnPathFound(Vector2[] waypoints, bool success)
        {
            if (success)
            {
                this.path = new Path(waypoints, position, turnDist);
                FollowPath();
            }
        }

        IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            //transform.LookAt(path.lookPoints[0])

            while (followingPath)
            {
                while (path.turnBoundaries[pathIndex].HasCrossedLine(position))
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        break;

                    } else
                    {
                        ++pathIndex;
                    }
                }
                if (followingPath)
                {
                    //rotate a little and move correctly
                }
            }
            yield return null;
        }


        //TODO
        IEnumerator UpdatePath()
        {
            Vector2 targetPosOld;
            while(true)
            {

            }
        }
    }


}

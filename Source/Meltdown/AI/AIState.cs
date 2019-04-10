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
using Meltdown.Components;

namespace Meltdown.AI
{
    abstract class AIState
    {
        protected PathRequestManager PathRequestManager
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<PathRequestManager>();
            }
        }
        protected Vector2 myPos;
        protected Vector2 target;
        protected Vector2 oldTarget;
        protected Path path;
        int turnDist = 1;
        const float sqrdUpdateThreshold = 5;
        const float minPathUpdateTime = .5f;

        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Entity entity, Time time);

        protected Vector2 Pathfinder(Vector2 playerPos, Vector2 position)
        {
            return playerPos - position;
        }

        protected void UpdatePath()
        {
            //ALSO insert a timed recheck
            if ((target - oldTarget).LengthSquared() > sqrdUpdateThreshold)
            { 
                PathRequestManager.RequestPath(this.myPos, target, OnPathFound);
                this.oldTarget = this.target;
            }
        }

        protected void OnPathFound(Vector2[] waypoints, bool success)
        {
            if (success)
            {
                this.path = new Path(waypoints, myPos, turnDist);
            }
        }
    }
}

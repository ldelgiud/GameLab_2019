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
using System.Diagnostics;
using tainicom.Aether.Physics2D.Collision;

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

        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Game1.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            }
        }

        protected Vector2 myPos;
        protected Vector2 target;
        Vector2 oldTarget;
        float timeOfLastUpdate; 
        protected Path path;
        int turnDist = 1;
        public const float updateThreshold = 1;
        public const float sqrdUpdateThreshold = AIState.updateThreshold * AIState.updateThreshold;
        const float minPathUpdateTime = 0.5f;
        const float maxPathUpdateTime = 2f;
        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Entity entity, Time time);

        protected void UpdatePath(Time time)
        {
            float sqrdDist = (target - oldTarget).LengthSquared();
            float timePassed = time.Absolute - this.timeOfLastUpdate;
            if ((sqrdDist > sqrdUpdateThreshold && timePassed > AIState.minPathUpdateTime)
                ||timePassed > AIState.maxPathUpdateTime)
            {
                //Debug.WriteLine("and Succeding");
                this.timeOfLastUpdate = time.Absolute;
                this.oldTarget = this.target;
                PathRequestManager.RequestPath(this.myPos, target, OnPathFound);
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

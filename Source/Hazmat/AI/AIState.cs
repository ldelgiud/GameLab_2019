using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;

using Hazmat.Utilities;
using Hazmat.Pathfinding;
using System.Collections;
using Hazmat.Components;
using System.Diagnostics;
using tainicom.Aether.Physics2D.Collision;

namespace Hazmat.AI
{
    abstract class AIState
    {
        protected PathRequestManager PathRequestManager
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<PathRequestManager>();
            }
        }

        public static QuadTree<Entity> quadtree
        {
            get
            {
                return Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
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


        
        protected bool IsInSight(Vector2 source, Vector2 target)
        {
            Vector2 dirvec = target - source;
            dirvec.Normalize();
            //Necessary to avoid starting inside the enemy!! Adjust factor
            source = source + dirvec * 1.5f;
            RayCastInput rayCastInput = new RayCastInput
            {
                MaxFraction = 1,
                Point1 = source,
                Point2 = target
            };
            bool isInSight = false;
            AIState.quadtree.RayCast((RayCastInput ray, Element<Entity> collidee) =>
            {
                //TODO: check this check
                if (!collidee.Value.Get<AABBComponent>().solid) return -1f;
                if (collidee.Value.Has<PlayerComponent>())
                {
                    isInSight = true;
                }
                return 0f;
            }, ref rayCastInput);

            return isInSight;
        }

        protected bool IsPathClear(AABBComponent aabb, Vector2 target)
        {
            Vector2 bottomLeft = aabb.element.Span.LowerBound;
            Vector2 topRight = aabb.element.Span.UpperBound;
            Vector2 bottomRight = new Vector2(topRight.X, bottomLeft.Y);
            Vector2 topLeft = new Vector2(bottomLeft.X, topRight.Y);
            return IsInSight(bottomLeft, target)
                && IsInSight(bottomRight, target)
                && IsInSight(topLeft, target)
                && IsInSight(topRight, target);
        }
    }
}

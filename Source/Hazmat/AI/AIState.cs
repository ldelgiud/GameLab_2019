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
using Hazmat.Utilities.Extensions;

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

        protected Entity me;
        protected Vector2 myPos;
        protected Vector2 target;
        protected Vector2 oldTarget;
        protected float oldLife;
        float timeOfLastAttack;
        float timeOfLastUpdate; 
        protected Path path;
        int turnDist = 1;
        public const float updateThreshold = 0.5f;
        public const float sqrdUpdateThreshold = AIState.updateThreshold * AIState.updateThreshold;
        const float minPathUpdateTime = 0.5f;
        const float maxPathUpdateTime = 1f;

        protected bool AmIMad(Time time)
        {
            bool wasAttacked = (this.me.Get<HealthComponent>().Health < this.oldLife);
            this.oldLife = this.me.Get<HealthComponent>().Health;
            float timeSinceLastAttack = time.Absolute - this.timeOfLastAttack;
            if (wasAttacked)
            {
                this.timeOfLastAttack = time.Absolute;
                return true;
            }
            else if (timeSinceLastAttack <= Constants.MEMORY_OF_HIT) return true;

            return false;
        }

        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Time time);

        protected float SqrdDist
        {
            get
            {
                return (this.myPos - this.target).LengthSquared();
            }
        }

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

        protected Vector2 FindClosestPlayer(List<PlayerInfo> playerInfos)
        {
            Vector2 myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            float minSqrdDist = 1000000;
            Vector2 closest =playerInfos[0].transform.Translation.ToVector2();
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 curr = player.transform.Translation.ToVector2();
                float sqrdDist = (curr - myPos).LengthSquared();

                if(minSqrdDist > sqrdDist)
                {
                    minSqrdDist = sqrdDist;
                    closest = curr;
                }
                
            }
            return closest;
        }

        protected void FollowTarget(float speed)
        {
            this.GoTo(this.target, speed);
        }

        protected void GoTo(Vector2 position, float speed)
        {
            ref VelocityComponent velocity = ref this.me.Get<VelocityComponent>();
            Vector2 newVel = (position - this.myPos);
            newVel.Normalize();
            velocity.velocity = newVel * speed;
            this.me.Get<Transform3DComponent>().value.Rotation = new Vector3(Vector2.Zero, newVel.ToRotation());
        }

        protected void OnPathFound(Vector2[] waypoints, bool success)
        {
            if (success)
            {
                this.path = new Path(waypoints, myPos, turnDist);
            }
        }
        
        protected bool IsTargetInSight()
        {
            return IsInSight(this.myPos, this.target);   
        }

        protected bool IsInSight(Vector2 src, Vector2 target)
        {
            RayCastInput rayCastInput = new RayCastInput
            {
                MaxFraction = 1,
                Point1 = src,
                Point2 = target
            };

            bool isInSight = false;

            AIState.quadtree.MyRayCast((RayCastInput ray, Element<Entity> collidee) =>
            {
                //if (collidee.Value.Equals(me)) return -1f;
                if (collidee.Value.Has<AABBComponent>())
                {
                    if (collidee.Value.Has<PlayerComponent>())
                    {
                        //Player found, return 
                        isInSight = true;
                        return 0f;
                    }
                    else if (collidee.Value.Get<AABBComponent>().solid) return 0f;
                }
                return 0.0f;
            }, ref rayCastInput);

            return isInSight;
        }
        
    }
}

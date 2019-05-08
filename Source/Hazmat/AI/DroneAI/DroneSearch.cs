using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using DefaultEcs;
using Hazmat.Components;
using System.Diagnostics;

namespace Hazmat.AI
{
    class DroneSearch : AIState
    {
        public DroneSearch(Entity me, Vector2 target)
        {
            this.me = me;
            this.target = target;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
        }


        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            //Update information about myself
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = FindClosestPlayer(playerInfos);
            ref VelocityComponent velocity = ref this.me.Get<VelocityComponent>();

            //Find closest player
            float sqrdDistance = (this.target - this.myPos).LengthSquared();

            //SEARCH
            this.UpdatePath(time);
            
            //STEP
            if (sqrdDistance <= Constants.DIRECT_ATTACK_SQRD_DIST)
            {
                this.FollowTarget(Constants.DRONE_SPEED);
            }
            else if (path != null)
            {
                (Vector2, Line) nextNode;
                bool followingPath;
                while (followingPath = path.bounds.TryPeek(out nextNode))
                {
                    if (nextNode.Item2.HasCrossedLine(myPos))
                    {
                        path.bounds.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
                if (followingPath)
                {
                    this.GoTo(nextNode.Item1, Constants.DRONE_SPEED);
                }

            }

            //UPDATE STATE
            if (sqrdDistance >= Constants.SEARCH_TO_STANDBY_SQRD_DIST)
            {
                velocity.velocity = Vector2.Zero;
                return new DroneStandby(this.me);
            }
            return this;
        }
    }
}

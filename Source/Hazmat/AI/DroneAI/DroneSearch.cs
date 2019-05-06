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



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            //Update information about myself
            Transform3DComponent transform = entity.Get<Transform3DComponent>();
            this.myPos = transform.value.Translation.ToVector2();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation.ToVector2() - this.myPos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            this.target = closestPlayer.transform.Translation.ToVector2();
            float sqrdDistance = (this.target - this.myPos).LengthSquared();

            //SEARCH
            //Debug.WriteLine("Drone Searching");

            this.UpdatePath(time);
            Vector2 newVel;
            //STEP
            if (this.IsPathClear(entity.Get<AABBComponent>(), this.target, entity) || 
                sqrdDistance <= Constants.DIRECT_ATTACK_SQRD_DIST)
            {
                newVel = (this.target - this.myPos);
                newVel.Normalize();
                velocity.velocity = newVel * Constants.DRONE_SPEED;
                transform.value.Rotation = new Vector3(Vector2.Zero, newVel.ToRotation());
            }
            else if (path != null)
            {
                (Vector2, Line) nextNode;
                bool followingPath;
                while (followingPath = path.bounds.TryPeek(out nextNode))
                {
                    if (nextNode.Item2.HasCrossedLine(this.myPos))
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
                    newVel = nextNode.Item1 - myPos;
                    newVel.Normalize();
                    velocity.velocity = newVel * Constants.DRONE_SPEED;
                    transform.value.Rotation = new Vector3(Vector2.Zero, newVel.ToRotation());
                }

            }


            //UPDATE STATE
            if (sqrdDistance >= Constants.SEARCH_TO_STANDBY_SQRD_DIST)
            {
                velocity.velocity = new Vector2(0);
                return new DroneStandby();
            }
            return this;
        }
    }
}

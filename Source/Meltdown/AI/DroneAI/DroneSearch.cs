using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using DefaultEcs;
using Meltdown.Components;
using System.Diagnostics;

namespace Meltdown.AI
{
    class DroneSearch : AIState
    {



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            //Update information about myself
            this.myPos = entity.Get<Transform2DComponent>().value.Translation;
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - this.myPos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            this.target = closestPlayer.transform.Translation;
            float sqrdDistance = (this.target - this.myPos).LengthSquared();

            //SEARCH
            //Debug.WriteLine("Drone Searching");

            this.UpdatePath(time);
            Vector2 newVel;

            //STEP
            if (this.IsPathClear(entity.Get<AABBComponent>(), this.target))
            {
                newVel = (this.target - this.myPos);
                newVel.Normalize();
                velocity.velocity = newVel * Constants.DRONE_SPEED;
            } else if (path != null)
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

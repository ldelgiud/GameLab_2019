using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using DefaultEcs;
using Hazmat.Components;
using System.Diagnostics;

namespace Hazmat.AI
{
    class ShooterSearch : AIState
    {

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            //Debug.WriteLine("Shooter Search");
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
            this.UpdatePath(time);
            if (path == null)
            {
                this.PathRequestManager.RequestPath(this.myPos, this.target, OnPathFound);
            }
            //STEP
            if (path != null)
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
                    Vector2 newVel = nextNode.Item1 - myPos;
                    newVel.Normalize();
                    velocity.velocity = newVel * Constants.DRONE_SPEED;

                }
            }
            //TODO: if raycasting hits player start already to shoot


            //UPDATE STATE
            if (sqrdDistance <= Constants.SEARCH_TO_ATTACK_SQRD_DIST && this.IsInSight(this.myPos, this.target))
            {
                velocity.velocity = new Vector2(0);
                //Debug.WriteLine("going into ATTACK");
                return new ShooterAttack();
            }
            if (sqrdDistance >= Constants.SEARCH_TO_STANDBY_SQRD_DIST)
            {
                velocity.velocity = new Vector2(0);
                //Debug.WriteLine("going into STANDBY");
                return new ShooterStandby();
            }
            return this;
        }
    }
}

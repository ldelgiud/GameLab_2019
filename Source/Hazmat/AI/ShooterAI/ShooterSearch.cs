﻿using System;
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
        public ShooterSearch(Entity me, Vector2 target, Time time)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = target;

            this.timeOfLastTotalUpdate = time.Absolute;
        }

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            if (time.Absolute < this.timeOfLastTotalUpdate + Constants.ENEMY_UPDATE_THRESHOLD) return this;

            this.timeOfLastTotalUpdate = time.Absolute;
            //Debug.WriteLine("Shooter Search");
            this.myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = this.FindClosestPlayer(playerInfos);
            
            //SEARCH
            this.UpdatePath(time);
            /*
            if (path == null)
            {
                this.PathRequestManager.RequestPath(this.myPos, this.target, OnPathFound);
            }*/

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
                if (followingPath) this.GoTo(nextNode.Item1, Constants.SHOOTER_SPEED);
            }

            //UPDATE STATE
            bool mad = this.AmIMad(time);
            if (mad) return new ShooterAttack(this.me, this.target, time);

            if (this.SqrdDist <= Constants.SEARCH_TO_ATTACK_SQRD_DIST && this.IsTargetInSight())
            {
                //Debug.WriteLine("going into ATTACK");
                return new ShooterAttack(this.me, this.target, time);
            }
            if (this.SqrdDist >= Constants.SEARCH_TO_STANDBY_SQRD_DIST)
            {
                //Debug.WriteLine("going into STANDBY");
                return new ShooterStandby(this.me, time);
            }
            return this;
        }
    }
}

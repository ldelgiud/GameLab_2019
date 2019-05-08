﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Hazmat.GameElements.Shooting;
using DefaultEcs;
using Hazmat.Components;
using System.Diagnostics;

namespace Hazmat.AI
{
    class ShooterAttack : AIState
    {
        public ShooterAttack(Entity me, Vector2 target)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = target;
        }
        
        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)

        {
            //Debug.WriteLine("Shooter Attack");
            this.myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = this.FindClosestPlayer(playerInfos);

            //MOVEMENT LOGIC
            if (this.SqrdDist >= 15*15)
            {
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
                    if (followingPath) this.GoTo(nextNode.Item1, Constants.SHOOTER_SPEED);
                }
            } else
            {
                ref var velocity = ref this.me.Get<VelocityComponent>();
                velocity.velocity = Vector2.Zero;
            }
            //ATTACK LOGIC
            Debug.Assert(this.me.Has<SmallGunComponent>());
            this.me.Get<SmallGunComponent>().Shoot(
                time.Absolute,
                this.me.Get<Transform3DComponent>().value,
                (this.target - this.myPos));

            //UPDATE STATE
            if (this.SqrdDist >= Constants.ATTACK_TO_SEARCH_SQRD_DIST || !this.IsTargetInSight())
            {
                //Debug.WriteLine("going into SEARCH");
                return new ShooterSearch(this.me, this.target);
            }
            return this;
        }
    }
}


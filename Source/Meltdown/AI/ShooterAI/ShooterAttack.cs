﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;
using Meltdown.GameElements.Shooting;
using DefaultEcs;
using Meltdown.Components;
using System.Diagnostics;

namespace Meltdown.AI
{
    class ShooterAttack : AIState
    {

        
        public override AIState UpdateState(
            List<PlayerInfo> playerInfos, 
            Entity entity,
            Time time) 
            
        {
            //Debug.WriteLine("Shooter Attack");
            this.myPos = entity.Get<Transform2DComponent>().value.Translation;
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - this.myPos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            this.target = closestPlayer.transform.Translation;
            Vector2 distVector = this.target - this.myPos;
            float sqrdDistance = distVector.LengthSquared();
            //MOVEMENT LOGIC
            if (sqrdDistance >= 100)
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
                    if (followingPath)
                    {
                        Vector2 newVel = nextNode.Item1 - myPos;
                        newVel.Normalize();
                        velocity.velocity = newVel * Constants.DRONE_SPEED;

                    }
                }
            } else
            {
                velocity.velocity = Vector2.Zero;
            }
            //ATTACK LOGIC
            Entity weapon = entity.Get<WeaponComponent>().weapon;
            Debug.Assert(weapon.Get<SmallGunComponent>() != null);
            weapon.Get<SmallGunComponent>().Shoot(
                time.Absolute, 
                weapon.Get<Transform2DComponent>().value,
                distVector);

            //UPDATE STATE
            if (sqrdDistance >= Constants.ATTACK_TO_SEARCH_SQRD_DIST || !this.IsInSight(this.myPos, this.target))
            {
                //Debug.WriteLine("going into SEARCH");
                return new ShooterSearch();
            }

            return this;
        }
    }
}


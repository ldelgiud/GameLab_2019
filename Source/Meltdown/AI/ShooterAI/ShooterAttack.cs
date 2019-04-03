﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;


namespace Meltdown.AI
{
    class ShooterAttack : AIState
    {

        const double distToSearch = 300;

        
        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - pos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = closestPlayer.transform.Translation - pos;
            //ATTACK!
            //TODO: implement attack



            //UPDATE STATE
            if (distVector.Length() >= distToSearch) return new ShooterSearch();
            if (distVector.Length() >= Constants.DIST_TO_STANDBY) return new ShooterStandby();
            return this;
        }
    }
}


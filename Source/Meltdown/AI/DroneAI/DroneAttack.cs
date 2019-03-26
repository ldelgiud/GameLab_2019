﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Utilities;
using Microsoft.Xna.Framework;

namespace Meltdown.AI
{
    class DroneAttack : AIState
    {

        const double distToStanby = 650;
        const double distToSearch = 250;

        
        public override AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Position - pos;
                if (dist.Length() < minDist) closestPlayer = player;
                
            }
            Vector2 distVector = closestPlayer.transform.Position - pos;
            //ATTACK!
            //TODO: implement attack



            //UPDATE STATE
            if (distVector.Length() >= DroneAttack.distToSearch) return new DroneSearch();
            if (distVector.Length() >= DroneAttack.distToStanby) return new StandbyState();
            return this;
        }
    }
}


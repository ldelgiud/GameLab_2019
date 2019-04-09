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
            Vector2 position = entity.Get<Transform2DComponent>().value.Translation;
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - position;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = closestPlayer.transform.Translation - position;
            //ATTACK LOGIC
            Entity weapon = entity.Get<WeaponComponent>().weapon;
            Debug.Assert(weapon.Get<SmallGunComponent>() != null);
            weapon.Get<SmallGunComponent>().Shoot(
                time.Absolute, 
                weapon.Get<Transform2DComponent>().value,
                distVector);

            //UPDATE STATE
            if (distVector.Length() >= Constants.ATTACK_TO_SEARCH_DIST) return new ShooterSearch();
            return this;
        }
    }
}


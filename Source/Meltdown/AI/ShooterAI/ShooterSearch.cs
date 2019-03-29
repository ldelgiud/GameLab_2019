using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.AI
{
    class ShooterSearch : AIState
    {
        const double distToAttack = 250.0;



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Vector2 pos,
            ref Vector2 velocity)
        {
            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.value.position.ToVector2() - pos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = Pathfinder(closestPlayer.transform.value.position.ToVector2(), pos);
            double distance = distVector.Length();
            //SEARCH
            distVector.Normalize();
            velocity = Vector2.Multiply(distVector, Constants.SHOOTER_SPEED);
            //TODO: Implement pathfinding method

            //UPDATE STATE
            if (distance <= distToAttack)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new ShooterAttack();
            }
            if (distance >= Constants.DIST_TO_STANDBY)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return new ShooterAttack();
            }
            return this;
        }
    }
}

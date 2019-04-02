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

namespace Meltdown.AI
{
    class DroneSearch : AIState
    {



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            WorldTransformComponent transform = entity.Get<WorldTransformComponent>();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();
            Vector2 position = transform.value.position.ToVector2();

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.value.position.ToVector2() - position;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = Pathfinder(closestPlayer.transform.value.position.ToVector2(), position);
            double distance = distVector.Length();
            //SEARCH
            distVector.Normalize();
            velocity.velocity = Vector2.Multiply(distVector, Constants.DRONE_SPEED);
            transform.value.SetRotationZ(MathF.Atan2(velocity.velocity.X, velocity.velocity.Y) + MathHelper.Pi);

            //TODO: Implement pathfinding method

            //UPDATE STATE
            if (distance >= Constants.SEARCH_TO_STANDBY_DIST)
            {
                velocity.velocity = new Vector2(0);
                return new DroneStandby();
            }
            return this;
        }
    }
}

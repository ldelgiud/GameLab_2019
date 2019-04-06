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
            Transform2DComponent transform = entity.Get<Transform2DComponent>();
            ref VelocityComponent velocity = ref entity.Get<VelocityComponent>();
            Vector2 position = transform.value.Translation;

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: NullCheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - position;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            Vector2 distVector = this.Pathfinder(closestPlayer.transform.Translation, position);
            double distance = distVector.Length();
            //SEARCH
            distVector.Normalize();
            velocity.velocity =distVector * Constants.DRONE_SPEED;
            transform.value.Rotation = velocity.velocity.ToRotation();

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

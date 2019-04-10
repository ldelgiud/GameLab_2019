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
using tainicom.Aether.Physics2D.Collision;
using System.Diagnostics;

namespace Meltdown.AI
{
    class DroneOffline : AIState
    {
        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            this.myPos = entity.Get<Transform2DComponent>().value.Translation;
            foreach (PlayerInfo player in playerInfos)
            {
                float sqrdDist = (player.transform.Translation - this.myPos).LengthSquared();
                if (sqrdDist <= Constants.OFFLINE_TO_STANDBY_DIST)
                    return new DroneStandby();
            }
            return this;
        }
    }
}

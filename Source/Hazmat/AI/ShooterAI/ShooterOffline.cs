using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using DefaultEcs;
using Hazmat.Components;
using tainicom.Aether.Physics2D.Collision;
using System.Diagnostics;

namespace Hazmat.AI
{
    class ShooterOffline : AIState
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
                if (sqrdDist <= Constants.OFFLINE_TO_STANDBY_SQRD_DIST)
                {
                    //Debug.WriteLine("going into standby");
                    return new ShooterStandby();

                }
            }
            return this;
        }
    }
}

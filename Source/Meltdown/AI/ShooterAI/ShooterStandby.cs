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
    class ShooterStandby : AIState
    {

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos, 
            Entity entity,
            Time time)
        {
            Vector2 position = entity.Get<Transform2DComponent>().value.Translation;

            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation - position;

                if (dist.Length() <= Constants.STANDBY_TO_SEARCH_DIST) return new ShooterSearch();
            }
            return this;
        }
    }
}

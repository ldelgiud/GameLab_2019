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
using System.Diagnostics;

namespace Meltdown.AI
{
    class DroneStandby : AIState
    {



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos, 
            Entity entity,
            Time time)
        {
            this.myPos = entity.Get<Transform2DComponent>().value.Translation;
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 distVec = player.transform.Translation - this.myPos;
                float sqrdDist = distVec.LengthSquared();
                this.target = player.transform.Translation;
                if (sqrdDist >= Constants.STANDBY_TO_OFFLINE_SQRD_DIST) return new DroneOffline();
                else if (sqrdDist <= Constants.STANDBY_TO_SEARCH_SQRD_DIST)
                    if (this.IsInSight(this.myPos, this.target))
                    {
                        Debug.WriteLine("DRONY: SAW YOU BITCH!!");
                        return new DroneSearch();
                    }
                    else if (sqrdDist <= Constants.BLIND_STANDBY_TO_SEARCH_SQRD_DIST)
                    {
                        Debug.WriteLine("DRONY: TOO CLOSE BITCH!!");
                        return new DroneSearch();
                    }
            }
            return this; ;
        }
    }
}

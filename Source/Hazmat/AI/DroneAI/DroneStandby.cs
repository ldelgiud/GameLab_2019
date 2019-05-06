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
using System.Diagnostics;

namespace Hazmat.AI
{
    class DroneStandby : AIState
    {



        public override AIState UpdateState(
            List<PlayerInfo> playerInfos, 
            Entity entity,
            Time time)
        {

            this.myPos = entity.Get<Transform3DComponent>().value.Translation.ToVector2();
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 distVec = player.transform.Translation.ToVector2() - this.myPos;
                float sqrdDist = distVec.LengthSquared();
                this.target = player.transform.Translation.ToVector2();

                if (sqrdDist >= Constants.STANDBY_TO_OFFLINE_SQRD_DIST) return new DroneOffline();
                else if (sqrdDist <= Constants.STANDBY_TO_SEARCH_SQRD_DIST)
                    if (this.IsInSight(this.myPos, this.target, entity))
                    {
                        Debug.WriteLine("DRONY: SAW YOU BITCH!!");
                        Debug.WriteLine("DISTANCE: " + Math.Sqrt(sqrdDist));
                        return new DroneSearch();
                    }
                    else if (sqrdDist <= Constants.BLIND_STANDBY_TO_SEARCH_SQRD_DIST)
                    {
                        Debug.WriteLine("DRONY: TOO CLOSE BITCH!!");
                        Debug.WriteLine("SQRD DISTANCE: " + Math.Sqrt(sqrdDist));
                        return new DroneSearch();
                    }
            }
            return this; ;
        }
    }
}

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
        public DroneStandby(Entity me, Time time)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.timeOfLastTotalUpdate = time.Absolute;
        }


        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            if (time.Absolute < this.timeOfLastTotalUpdate + Constants.ENEMY_UPDATE_THRESHOLD) return this;

            this.timeOfLastTotalUpdate = time.Absolute;
            //TODO: might be a problem with 2 or more players
            this.target = this.FindClosestPlayer(playerInfos);
            this.myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            float sqrdDist = (this.myPos - this.target).LengthSquared();

            //UPDATE STATE
            bool mad = this.AmIMad(time);
            if (mad) return new DroneSearch(this.me, this.target, time);
            if (sqrdDist >= Constants.STANDBY_TO_OFFLINE_SQRD_DIST) return new DroneOffline(this.me, time);
            else if (sqrdDist <= Constants.STANDBY_TO_SEARCH_SQRD_DIST)
            {
                if (this.IsTargetInSight())
                {
                    //Debug.WriteLine("DRONY: SAW YOU BITCH!!");
                    //Debug.WriteLine("DISTANCE: " + Math.Sqrt(sqrdDist));
                    return new DroneSearch(this.me, this.target, time);
                }
                else if (sqrdDist <= Constants.BLIND_STANDBY_TO_SEARCH_SQRD_DIST)
                {
                    //Debug.WriteLine("DRONY: TOO CLOSE BITCH!!");
                    //Debug.WriteLine("SQRD DISTANCE: " + Math.Sqrt(sqrdDist));
                    return new DroneSearch(this.me, this.target, time);
                }
            }
            return this;
        }
    }
}

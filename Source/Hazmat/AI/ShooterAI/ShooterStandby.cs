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
    class ShooterStandby : AIState
    {
        public ShooterStandby(Entity me)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();

            ref var vel = ref me.Get<VelocityComponent>();
            vel.velocity = Vector2.Zero;
        }

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = FindClosestPlayer(playerInfos);

            bool mad = this.AmIMad(time);
            if (mad) return new ShooterAttack(this.me, this.target);

            if (this.SqrdDist >= Constants.STANDBY_TO_OFFLINE_SQRD_DIST)
            {
                //Debug.WriteLine("going into offline");
                return new ShooterOffline(this.me);
            }
            else if (this.SqrdDist <= Constants.STANDBY_TO_SEARCH_SQRD_DIST)
            {
                if (this.IsTargetInSight())
                {
                    //Debug.WriteLine("SHOOTY MC FACE: SAW YOU!!");
                    return new ShooterSearch(this.me, this.target);
                }
                else if (this.SqrdDist <= Constants.BLIND_STANDBY_TO_SEARCH_SQRD_DIST)
                {
                    //Debug.WriteLine("SHOOTY MC FACE: TOO CLOSE");
                    return new ShooterSearch(this.me, this.target);
                }
            }
            return this;
        }
    }
}

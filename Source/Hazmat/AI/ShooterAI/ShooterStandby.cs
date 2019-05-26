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
        public ShooterStandby(Entity me, Time time)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();

            ref var vel = ref me.Get<VelocityComponent>();
            vel.velocity = Vector2.Zero;

            this.timeOfLastTotalUpdate = time.Absolute;
        }

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            if (time.Absolute < this.timeOfLastTotalUpdate + Constants.ENEMY_UPDATE_THRESHOLD) return this;

            this.timeOfLastTotalUpdate = time.Absolute;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = FindClosestPlayer(playerInfos);

            bool mad = this.AmIMad(time);
            if (mad)
            {
                return new ShooterAttack(this.me, this.target, time);
                Debug.WriteLine("You Hit me!");
            }

            if (this.SqrdDist >= Constants.STANDBY_TO_OFFLINE_SQRD_DIST)
            {
                //Debug.WriteLine("going into offline");
                return new ShooterOffline(this.me, time);
            }
            else if (this.SqrdDist <= Constants.STANDBY_TO_SEARCH_SQRD_DIST)
            {
                if (this.IsTargetInSight())
                {
                    Debug.WriteLine("SHOOTY MC FACE: SAW YOU!!");
                    Debug.Write("Distance: " + MathF.Sqrt(this.SqrdDist));
                    return new ShooterSearch(this.me, this.target, time);
                }
                else if (this.SqrdDist <= Constants.BLIND_STANDBY_TO_SEARCH_SQRD_DIST)
                {
                    Debug.WriteLine("SHOOTY MC FACE: TOO CLOSE");
                    return new ShooterSearch(this.me, this.target, time);
                }
            }
            return this;
        }
    }
}

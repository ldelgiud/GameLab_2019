﻿using System;
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
        public ShooterOffline(Entity me, Time time)
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
            float sqrdDist = (this.myPos - this.FindClosestPlayer(playerInfos)).LengthSquared();

            if (sqrdDist <= Constants.OFFLINE_TO_STANDBY_SQRD_DIST)
                return new ShooterStandby(this.me, time);
            else return this;
        }
    }
}

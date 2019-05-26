using DefaultEcs;
using Hazmat.AI;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;
using Mazmat.AI.MailboxAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazmat.AI.MailBoxAI
{
    class MailboxOffline : AIState
    {
        public MailboxOffline(Entity me, Time time)
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
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            float sqrdDist = (this.myPos - this.FindClosestPlayer(playerInfos)).LengthSquared();

            if (sqrdDist <= Constants.OFFLINE_TO_ATTACK_SQRD_DIST)
                return new MailboxAttack(this.me, this.target, time);
            else return this;
        }
    }
}

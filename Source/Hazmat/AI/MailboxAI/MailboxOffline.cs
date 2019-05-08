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
        public MailboxOffline(Entity me)
        {
            this.me = me;
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
        }

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)
        {
            this.myPos = me.Get<Transform3DComponent>().value.Translation.ToVector2();
            float sqrdDist = (this.myPos - this.FindClosestPlayer(playerInfos)).LengthSquared();

            if (sqrdDist <= Constants.OFFLINE_TO_ATTACK_SQRD_DIST)
                return new MailboxAttack(this.me, this.target);
            else return this;
        }
    }
}

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

        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)
        {
            this.myPos = entity.Get<Transform3DComponent>().value.Translation.ToVector2();
            foreach (PlayerInfo player in playerInfos)
            {
                float sqrdDist = (player.transform.Translation.ToVector2() - this.myPos).LengthSquared();
                if (sqrdDist <= Constants.OFFLINE_TO_ATTACK_SQRD_DIST)
                    //entity.SetModelAnimation("Armature|Shooting");
                    return new MailboxAttack();
            }
            return this;
        }
    }
}

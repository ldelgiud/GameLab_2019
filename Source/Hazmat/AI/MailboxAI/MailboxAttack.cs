using DefaultEcs;
using Hazmat.AI;
using Hazmat.Components;
using Hazmat.Utilities;
using System;
using System.Collections.Generic;
using Hazmat.Utilities.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Hazmat.AI.MailBoxAI;

namespace Mazmat.AI.MailboxAI
{
    class MailboxAttack : AIState
    {
        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Entity entity,
            Time time)

        {
            //Debug.WriteLine("Mailbox Attack");
            this.myPos = entity.Get<Transform3DComponent>().value.Translation.ToVector2();

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!
            PlayerInfo closestPlayer = playerInfos[0];
            foreach (PlayerInfo player in playerInfos)
            {
                Vector2 dist = player.transform.Translation.ToVector2() - this.myPos;
                if (dist.Length() < minDist) closestPlayer = player;

            }
            this.target = closestPlayer.transform.Translation.ToVector2();
            Vector2 distVector = this.target - this.myPos;
            float sqrdDistance = distVector.LengthSquared();
            //MOVEMENT LOGIC
            //No Movement only rotation
            Transform3DComponent transform = entity.Get<Transform3DComponent>();
            transform.value.Rotation = new Vector3(Vector2.Zero, distVector.ToRotation());

            //ATTACK LOGIC
            Debug.Assert(entity.Has<SmallGunComponent>());
            entity.Get<SmallGunComponent>().Shoot(
                time.Absolute,
                entity.Get<Transform3DComponent>().value,
                distVector);

            //UPDATE STATE
            if (sqrdDistance >= Constants.ATTACK_TO_SEARCH_SQRD_DIST || !this.IsInSight(this.myPos, this.target, entity))
            {
                //Debug.WriteLine("going into SEARCH");
                return new MailboxOffline();
            }

            return this;
        }
    }
}

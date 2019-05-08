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
        public MailboxAttack(Entity me, Vector2 target)
        {
            this.me = me;
            this.target = target;
        }


        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)

        {
            //Debug.WriteLine("Mailbox Attack");
            this.myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = this.FindClosestPlayer(playerInfos);

            //Find closest player
            double minDist = Double.MaxValue;
            //TODO: Nullcheck next line!!

            Vector2 distVector = this.target - this.myPos;
            float sqrdDistance = distVector.LengthSquared();
            //MOVEMENT LOGIC
            //No Movement only rotation
            Transform3DComponent transform = this.me.Get<Transform3DComponent>();
            transform.value.Rotation = new Vector3(Vector2.Zero, distVector.ToRotation());

            //ATTACK LOGIC
            Debug.Assert(this.me.Has<SmallGunComponent>());
            this.me.Get<SmallGunComponent>().Shoot(
                time.Absolute,
                this.me.Get<Transform3DComponent>().value,
                distVector);

            //UPDATE STATE
            if (sqrdDistance >= Constants.ATTACK_TO_SEARCH_SQRD_DIST || !this.IsTargetInSight())
            {
                //Debug.WriteLine("going into SEARCH");
                return new MailboxOffline(this.me);
            }

            return this;
        }
    }
}

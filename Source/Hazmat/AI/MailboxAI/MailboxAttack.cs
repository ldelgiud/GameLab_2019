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
        public MailboxAttack(Entity me, Vector2 target, Time time)
        {
            this.me = me;
            this.target = target;
            this.timeOfLastTotalUpdate = time.Absolute;
        }


        public override AIState UpdateState(
            List<PlayerInfo> playerInfos,
            Time time)

        {
            if (time.Absolute < this.timeOfLastTotalUpdate + Constants.ENEMY_UPDATE_THRESHOLD) return this;

            this.timeOfLastTotalUpdate = time.Absolute;
            //Debug.WriteLine("Mailbox Attack");
            this.myPos = this.me.Get<Transform3DComponent>().value.Translation.ToVector2();
            this.target = this.FindClosestPlayer(playerInfos);

          
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
            if (sqrdDistance >= Constants.ATTACK_TO_OFFLINE_SQRD_DIST)
            {
                //Debug.WriteLine("going into SEARCH");
                return new MailboxOffline(this.me, time);
            }

            return this;
        }
    }
}

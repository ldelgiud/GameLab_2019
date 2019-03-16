using Meltdown.Components;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nez;

namespace Meltdown.Systems
{
    class AISystem : EntityProcessingSystem
    {

        List<PlayerInfo> playerInfo;

        public AISystem(List<PlayerInfo> playerInfo, Matcher matcher) :
            base ( matcher )
        {
            this.playerInfo = playerInfo;
        }

        
        
   

        public override void process(Entity entity)
        {
           
            var aiComponent = entity.getComponent<AIComponent>();
            var position = entity.getComponent<PositionComponent>();
            var velocity = entity.getComponent<VelocityComponent>();

            aiComponent.State =
                aiComponent.State.UpdateState(playerInfo, position.position, ref velocity.velocity);

            
        }
    }
}

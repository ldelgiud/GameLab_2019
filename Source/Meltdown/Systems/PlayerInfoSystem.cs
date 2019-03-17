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
    class PlayerInfoSystem : EntityProcessingSystem
    {

        List<PlayerInfo> playerInfo;

        public PlayerInfoSystem(List<PlayerInfo> playerInfo, Matcher matcher) : 
            base( matcher)
        {
            this.playerInfo = playerInfo;
        }
        

        public override void process(Entity entity)
        {
            var player = entity.getComponent<PlayerComponent>();
            var position = entity.getComponent<PositionComponent>();


            this.playerInfo[player.Id].Update(position.position);
            
        }
    }
}

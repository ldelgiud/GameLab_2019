using Meltdown.Components;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        

        public override void Update(GameTime gameTime)
        {
            foreach (int id in this.ActiveEntities)
            {
                PlayerComponent player = this.playerMapper.Get(id);
                PositionComponent position = this.positionMapper.Get(id);
                this.playerInfo[player.Id].Update(position.position);
            }
        }
    }
}

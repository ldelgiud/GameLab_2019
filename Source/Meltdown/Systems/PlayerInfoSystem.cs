using Meltdown.Components;
using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Systems
{
    class PlayerInfoSystem : EntityUpdateSystem
    {

        List<PlayerInfo> playerInfo;
        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<PlayerComponent> playerMapper;

        public PlayerInfoSystem(List<PlayerInfo> playerInfo) : 
            base(Aspect.All(typeof(PlayerComponent),typeof(PositionComponent)))
        {
            this.playerInfo = playerInfo;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.playerMapper = mapperService.GetMapper<PlayerComponent>();
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
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

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
    class AISystem : EntityUpdateSystem
    {

        List<PlayerInfo> playerInfo;
        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<VelocityComponent> velocityMapper;
        ComponentMapper<AIComponent> aiMapper;

        public AISystem(List<PlayerInfo> playerInfo) :
            base(Aspect.All(typeof(AIComponent), typeof(VelocityComponent), typeof(PositionComponent)))
        {
            this.playerInfo = playerInfo;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
            this.velocityMapper = mapperService.GetMapper<VelocityComponent>();
            this.aiMapper = mapperService.GetMapper<AIComponent>();
        }


        
        public override void Update(GameTime gameTime)
        {
            foreach (int id in this.ActiveEntities)
            {
                AIComponent aiComponent = this.aiMapper.Get(id);
                PositionComponent position = this.positionMapper.Get(id);
                VelocityComponent velocity = this.velocityMapper.Get(id);
                
                aiComponent.State = 
                    aiComponent.State.UpdateState(playerInfo, position.position, ref velocity.velocity);

            }
        }
    }
}

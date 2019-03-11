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
        

        public AISystem(List<PlayerInfo> playerInfo) :
            base(Aspect.All(typeof(AIComponent), typeof(ActiveComponent)))
        {
            this.playerInfo = playerInfo;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            throw new NotImplementedException();
        }

        
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}

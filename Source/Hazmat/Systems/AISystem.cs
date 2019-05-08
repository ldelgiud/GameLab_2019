using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class AISystem : AEntitySystem<Time>
    {

        EntitySet players;

        public AISystem(World world) : base(
            world.GetEntities()
            .With<Transform3DComponent>()
            .With<WorldSpaceComponent>()
            .With<AIComponent>()
            .Build())
        {
            this.players = world.GetEntities().With<PlayerComponent>().Build();
        }

        protected override void Update(Time state, ReadOnlySpan<Entity> entities)
        {
            List<PlayerInfo> playerInfos = new List<PlayerInfo>();

            foreach(Entity entity in this.players.GetEntities())
            {
                playerInfos.Add(new PlayerInfo(
                    entity.Get<Transform3DComponent>().value,
                    entity.Get<PlayerComponent>().Id));
            }

            foreach(Entity entity in entities)
            {
                ref AIComponent aIState = ref entity.Get<AIComponent>();
                aIState.State =
                    aIState.State.UpdateState(playerInfos, state);
            }
            

        }
    }
}

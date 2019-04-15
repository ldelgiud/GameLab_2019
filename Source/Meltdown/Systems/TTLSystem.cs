using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class TTLSystem : AEntitySystem<Time>
    {

        public TTLSystem(World world) : base(
            world.GetEntities()
            .With<TTLComponent>()
            .Build())
        {

        }

        protected override void Update(Time state, in Entity entity)
        {
            ref TTLComponent TTL = ref entity.Get<TTLComponent>();
            if(TTL.decrease(state.Delta))
            {
                //TODO: ask if this is correct
                entity.Delete();
            }
        }
    }
}

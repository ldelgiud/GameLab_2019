using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Meltdown.Collision;
using Meltdown.Components;

namespace Meltdown.Systems
{
    sealed class InputSystem : AEntitySystem<GameTime>
    {
        public InputSystem(World world) : base(world.GetEntities().With<InputComponent>().Build()) { }

        protected override void Update(GameTime state, in Entity entity)
        {
            ref InputComponent inpComp = ref entity.Get<InputComponent>();
            inpComp.HandleInput();
        }
    }
    
}

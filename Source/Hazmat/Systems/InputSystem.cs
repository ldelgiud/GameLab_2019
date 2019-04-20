using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Collision;
using Hazmat.Utilities;
using Hazmat.Components;
using Hazmat.Input;

namespace Hazmat.Systems
{
    sealed class InputSystem : AEntitySystem<Time>
    {
        InputManager inputManager;
        public InputSystem(World world, InputManager inputManager) : base(world.GetEntities().With<InputComponent>().Build())
        {
            this.inputManager = inputManager;
        }

        protected override void Update(Time time, in Entity entity)
        {
            ref InputComponent input = ref entity.Get<InputComponent>();
            input.HandleInput(inputManager, time, entity);
        }
    }
    
}

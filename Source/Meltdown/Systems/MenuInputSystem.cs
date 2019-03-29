using System;

using Microsoft.Xna.Framework.Input;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.State;
using Meltdown.States;
using Meltdown.Utilities;
using Meltdown.Input;

namespace Meltdown.Systems
{
    class MenuInputSystem : AEntitySystem<Time>
    {
        InputManager inputManager;
        StateTransition transition;

        public MenuInputSystem(World world, InputManager inputManager, StateTransition transition) : base(
            world.GetEntities()
            //.With<>()
            .Build()
            )
        {
            this.inputManager = inputManager;
            this.transition = transition;
        }

        protected override void Update(Time state, ReadOnlySpan<Entity> entities)
        {
            switch (this.inputManager.GetEvent(Keys.Enter))
            {
                case PressEvent _:
                case ReleaseEvent _:
                    this.transition.Transition = new PushStateTransition(new GameState());
                    break;
                
            }
        }

    }
}

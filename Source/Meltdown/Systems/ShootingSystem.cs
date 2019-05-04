using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Microsoft.Xna.Framework.Input;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Input;
using Meltdown.Utilities.Extensions;


namespace Meltdown.Systems
{
    class ShootingSystem : AEntitySystem<Time>
    {

        World world;
        InputManager inputManager;

        public ShootingSystem(World world, InputManager inputManager) : base(
            world.GetEntities()
            .With<Transform2DComponent>()
            .With<WorldSpaceComponent>()
            .With<InputComponent>()
            .With<SmallGunComponent>()
            .Build())
        {
            this.world = world;
            this.inputManager = inputManager;
        }

        // Check for shoot button and shoot
        protected override void Update(Time time, in Entity entity)
        {
           // ref InputComponent inputComponent = ref entity.Get<InputComponent>();
            //inputComponent.HandleInput(inputManager, time, entity);

            // NOT NECESSARY -> Same as InputSystem

        }
    }
}

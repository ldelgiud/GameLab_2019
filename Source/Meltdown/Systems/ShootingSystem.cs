using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using DefaultEcs;
using DefaultEcs.System;

using Microsoft.Xna.Framework.Input;

using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;


namespace Meltdown.Systems
{
    class ShootingSystem : AEntitySystem<Time>
    {

        World world;

        public ShootingSystem(World world) : base(
            world.GetEntities()
            .With<WorldTransformComponent>()
            .With<SmallGunComponent>()
            .Build())
        {
            this.world = world;
        }

        // Check for shoot button and shoot
        protected override void Update(Time time, in Entity entity)
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            if (kState.IsKeyDown(Keys.F))
            {
                ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
                ref SmallGunComponent smallGun = ref entity.Get<SmallGunComponent>();

                Vector2 dir = mState.Position.ToVector2() - transform.Position; // not really needed?

                smallGun.Shoot(time.Absolute, transform, dir, world);
            }

        }
    }
}

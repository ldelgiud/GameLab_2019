using System;
using System.Diagnostics;
using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Graphics;
using Meltdown.Components;
using Meltdown.Utilities;

using Microsoft.Xna.Framework.Input;

namespace Meltdown.Systems
{
    class CameraSystem : ISystem<Time>
    {
        Camera worldCamera;
        EntitySet players;

        public bool IsEnabled { get; set; } = true;

        public CameraSystem(Camera worldCamera, World world)
        {
            this.worldCamera = worldCamera;
            this.players = world.GetEntities().With<PlayerComponent>().With<WorldTransformComponent>().Build();
        }

        public void Update(Time state)
        {
            var players = this.players.GetEntities();

            if (players.Length != 0)
            {
                var player = players[0];
                var transform = player.Get<WorldTransformComponent>();
                this.worldCamera.Transform.SetPositionX(transform.value.position.X);
                this.worldCamera.Transform.SetPositionY(transform.value.position.Y);
            }
        }

        public void Dispose()
        {
        }
    }
}

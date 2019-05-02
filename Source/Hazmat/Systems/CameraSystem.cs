using System;
using System.Diagnostics;
using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;

using Microsoft.Xna.Framework.Input;

namespace Hazmat.Systems
{
    class CameraSystem : ISystem<Time>
    {
        Camera3D worldCamera;
        EntitySet players;

        public bool IsEnabled { get; set; } = true;

        public CameraSystem(Camera3D worldCamera, World world)
        {
            this.worldCamera = worldCamera;
            this.players = world.GetEntities().With<PlayerComponent>().With<Transform3DComponent>().With<WorldSpaceComponent>().Build();
        }

        public void Update(Time state)
        {
            var players = this.players.GetEntities();

            if (players.Length != 0)
            {
                var player = players[0];
                var transform = player.Get<Transform3DComponent>();
                this.worldCamera.Transform.LocalTranslation = transform.value.LocalTranslation;
            }
        }

        public void Dispose()
        {
        }
    }
}

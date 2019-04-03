using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Meltdown.Graphics;
using Meltdown.Components;
using Meltdown.Utilities;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Systems
{
    class ModelDrawSystem : AEntitySystem<Time>
    {
        Camera2D worldCamera;

        public ModelDrawSystem(Camera2D worldCamera, World world) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<Transform3DComponent>()
            .Build()
            )
        {
            this.worldCamera = worldCamera;
        }

        protected override void Update(Time state, in Entity entity)
        {
            //ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            //ref ModelComponent model = ref entity.Get<ModelComponent>();

            //var transformMatrix = transform.value.GlobalTransform;

            //foreach (var mesh in model.value.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = Matrix.Transpose(transform.value.GlobalTransform);
            //        effect.View = Matrix.Transpose(this.worldCamera.View);
            //        effect.Projection = Matrix.Transpose(this.worldCamera.Projection);
            //    }

            //    mesh.Draw();
            //}

        }
    }
}

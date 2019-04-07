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
        Camera2D camera;

        public ModelDrawSystem(Camera2D camera, World world) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<Transform2DComponent>()
            .Build()
            )
        {
            this.camera = camera;
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref Transform2DComponent transform = ref entity.Get<Transform2DComponent>();
            ref ModelComponent model = ref entity.Get<ModelComponent>();

            var transformMatrix = transform.value.TransformMatrix;

            var m = transformMatrix.ToMatrix();
            var v = Matrix.CreateLookAt(new Vector3(this.camera.Transform.Translation, 50), this.camera.Transform.Translation.ToVector3(), Vector3.UnitY);
            var p = Matrix.CreateOrthographic(this.camera.ScreenWidth, this.camera.ScreenHeight, 0, 100);

            foreach (var mesh in model.value.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = m;
                    effect.View = v;
                    effect.Projection = p;
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

        }
    }
}

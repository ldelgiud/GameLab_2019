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

            var cameraPosition = this.camera.Transform.Translation;

            var m =
                Matrix.CreateScale(new Vector3(transform.value.Scale, 1) * model.info.scale) *
                Matrix.CreateRotationZ(transform.value.Rotation + model.info.rotation) *
                Matrix.CreateRotationX(-MathF.PI / 6) *
                Matrix.CreateTranslation(new Vector3(Camera2D.WorldToPerspective(transform.value.Translation), 0));

            var v = Matrix.CreateLookAt(new Vector3(Camera2D.WorldToPerspective(cameraPosition), 50), Camera2D.WorldToPerspective(this.camera.Transform.Translation).ToVector3(), Vector3.UnitY);
            var p = Matrix.CreateOrthographic(this.camera.ScreenWidth, this.camera.ScreenHeight, 0, 100);

            foreach (var mesh in model.value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    Effect effect = part.Effect;

                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = m;
                        ((BasicEffect)effect).View = v;
                        ((BasicEffect)effect).Projection = p;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        effect.Parameters["World"].SetValue(m);
                        effect.Parameters["View"].SetValue(v);
                        effect.Parameters["Projection"].SetValue(p);

                        Matrix worldInverseTransform = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * m));
                        effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransform);
                    }
                }

                mesh.Draw();
            }

        }
    }
}

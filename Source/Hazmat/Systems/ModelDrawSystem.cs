using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Animation;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Systems
{
    class ModelDrawSystem : AEntitySystem<Time>
    {
        GraphicsDevice graphicsDevice;
        Camera3D camera;

        public ModelDrawSystem(GraphicsDevice graphicsDevice, Camera3D camera, World world) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<Transform3DComponent>()
            .Build()
            )
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();
            ref ModelComponent model = ref entity.Get<ModelComponent>();

            var m = Matrix.CreateScale(model.info.scale) *
                Matrix.CreateRotationZ(model.info.rotation.Z) *
                Matrix.CreateRotationY(model.info.rotation.Y) *
                Matrix.CreateRotationX(model.info.rotation.X) *
                Matrix.CreateTranslation(model.info.translation) *
                transform.value.TransformMatrix;
            var v = this.camera.View;
            var p = this.camera.Projection;

            Animations animations = entity.Has<ModelAnimationComponent>() ? entity.Get<ModelAnimationComponent>().animations : null;

            foreach (var mesh in model.value.Meshes)
            {
                this.graphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (var part in mesh.MeshParts)
                {
                    if (animations != null)
                    {
                        part.UpdateVertices(animations.AnimationTransforms);
                    }

                    Effect effect = part.Effect;

                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = m;
                        ((BasicEffect)effect).View = v;
                        ((BasicEffect)effect).Projection = p;

                        ((BasicEffect)effect).Alpha = 1;
                    }
                    else
                    {
                        model.UpdateEffects(effect, state.Absolute);
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

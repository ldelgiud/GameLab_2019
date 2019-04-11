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
        Camera worldCamera;

        Effect effect;

        public ModelDrawSystem(Camera worldCamera, World world, Effect effect) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<WorldTransformComponent>()
            .Build()
            )
        {
            this.worldCamera = worldCamera;
            this.effect = effect;
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            ref ModelComponent model = ref entity.Get<ModelComponent>();
            
            var transformMatrix = transform.value.GlobalTransform;

            foreach (var mesh in model.value.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Transpose(transform.value.GlobalTransform * Matrix.CreateRotationX(MathHelper.PiOver2));
                    effect.View = Matrix.Transpose(this.worldCamera.View);
                    effect.Projection = Matrix.Transpose(this.worldCamera.Projection);
                }

                mesh.Draw();
            }


            //foreach (ModelMesh mesh in model.value.Meshes)
            //{
            //    foreach (ModelMeshPart part in mesh.MeshParts)
            //    {
            //        part.Effect = this.effect;
            //       // this.effect.Parameters["World"].SetValue(Matrix.Transpose(transform.value.GlobalTransform * Matrix.CreateRotationX(MathHelper.PiOver2)));
            //      //  this.effect.Parameters["View"].SetValue(Matrix.Transpose(this.worldCamera.View));
            //      //  this.effect.Parameters["Projection"].SetValue(Matrix.Transpose(this.worldCamera.Projection));
            //        this.effect.Parameters["u_blurSize"].SetValue(0.02f);
            //        this.effect.Parameters["u_intensity"].SetValue(2.0f);
            //    }

            //    mesh.Draw();
            //}


        }
    }
}

using System.Collections.Generic;
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
        List<Vector3> diffuseColors;
        bool doItOnce;

        public ModelDrawSystem(Camera worldCamera, World world, Effect effect) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<WorldTransformComponent>()
            .Build()
            )
        {
            this.worldCamera = worldCamera;
            this.effect = effect;
            diffuseColors = new List<Vector3>();
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref WorldTransformComponent transform = ref entity.Get<WorldTransformComponent>();
            ref ModelComponent model = ref entity.Get<ModelComponent>();
            
            var transformMatrix = transform.value.GlobalTransform;


            Matrix world = Matrix.Transpose(transform.value.GlobalTransform * Matrix.CreateRotationX(MathHelper.PiOver2));
            Matrix view = Matrix.Transpose(this.worldCamera.View);
            Matrix proj = Matrix.Transpose(this.worldCamera.Projection);

            //foreach (var mesh in model.value.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;
            //        //effect.EmissiveColor = new Vector3(1,0,0);
            //        //effect.PreferPerPixelLighting = true;
            //        effect.VertexColorEnabled = false;
            //    }

            //    mesh.Draw();
            //}


            //Texture2D texture = ((BasicEffect)model.value.Meshes[0].Effects[0]).Texture;

              if (!doItOnce)
              {
                foreach (ModelMesh mesh in model.value.Meshes) // hack to allow to get the right color for each mesh -> how can we get vertex colors?
                {
                    // GET INFO AND SET EFFECT
                    foreach (BasicEffect effect in mesh.Effects)
                        diffuseColors.Add(effect.DiffuseColor);
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                        meshPart.Effect = this.effect.Clone();
                }
                doItOnce = true;
              }

            int count = 0;
            foreach (ModelMesh mesh in model.value.Meshes)
            {
                // APPLY EFFECT AND SET PARAMETERS
                foreach (Effect myEffect in mesh.Effects)
                {

                    Matrix worldInverseTransform = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));

                    myEffect.Parameters["DiffuseColor"].SetValue(diffuseColors[count++]);
                    myEffect.Parameters["World"].SetValue(world);
                    myEffect.Parameters["View"].SetValue(view);
                    myEffect.Parameters["Projection"].SetValue(proj); 
                 //   myEffect.Parameters["AmbientIntensity"].SetValue(0.1f);
                    myEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransform);
                }
                mesh.Draw();
            }

            //foreach (ModelMesh mesh in model.value.Meshes)
            //{
            //    foreach (ModelMeshPart part in mesh.MeshParts)
            //    {
            //        this.effect.Parameters["World"].SetValue(world);
            //        this.effect.Parameters["View"].SetValue(view);
            //        this.effect.Parameters["Projection"].SetValue(proj);
            //        part.Effect = this.effect;

            //        Matrix worldInverseTransform = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));

            //        this.effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransform);

            //        //this.effect.Parameters["u_blurSize"].SetValue(0.02f);
            //        //this.effect.Parameters["u_intensity"].SetValue(2.0f);
            //    }

            //    mesh.Draw();
            //}


        }
    }
}

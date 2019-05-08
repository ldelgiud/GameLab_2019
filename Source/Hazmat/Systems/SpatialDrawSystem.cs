using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using Spine;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class SpatialDrawSystem : ISystem<Time>, IDisposable
    {
        GraphicsDevice graphicsDevice;
        World world;
        Camera3D camera;
        QuadTree<Entity> quadtree;

        SkeletonRenderer skeletonRenderer;

        Texture2D healthBar_W;
        Texture2D healthBar_G;
        BasicEffect effect;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public bool IsEnabled { get; set; } = true;

        public SpatialDrawSystem(GraphicsDevice graphicsDevice, World world, Camera3D camera, Texture2D healthBar_W, Texture2D healthBar_G)
        {
            this.graphicsDevice = graphicsDevice;
            this.world = world;
            this.camera = camera;
            this.quadtree = Hazmat.Instance.ActiveState.GetInstance<QuadTree<Entity>>();
            this.skeletonRenderer = new SkeletonRenderer(graphicsDevice);

            this.healthBar_W = healthBar_W;
            this.healthBar_G = healthBar_G;

            this.effect = new BasicEffect(graphicsDevice);

            this.vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 0)),
            });

            this.indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(new short[] {
                0, 1, 2,
                0, 2, 3,
            });
        }

        List<Entity> modelList = new List<Entity>();
        public void DrawModels(Time time)
        {
            foreach (var entity in modelList)
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


                foreach (var mesh in model.value.Meshes)
                {
                    this.graphicsDevice.DepthStencilState = DepthStencilState.Default;

                    foreach (var part in mesh.MeshParts)
                    {
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
                            model.UpdateEffects(effect, time.Absolute);
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

            this.modelList.Clear();
        }

        List<Entity> skeletonList = new List<Entity>();
        public void DrawSpineSkeletons(Time time)
        {
            var v = this.camera.View;
            var p = this.camera.Projection;

            ((BasicEffect)this.skeletonRenderer.Effect).Projection = p;
            ((BasicEffect)this.skeletonRenderer.Effect).View = v;

            foreach (var entity in this.skeletonList)
            {
                this.skeletonRenderer.Begin();
                this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
                ref var skeleton = ref entity.Get<SpineSkeletonComponent>();
                ref var transform = ref entity.Get<Transform3DComponent>();

                // Custom model matrix to Billboard the skeleton to the screen so that its not flat
                var m =
                    Matrix.CreateScale(skeleton.info.scale * transform.value.Scale * new Vector3(-1, 1, 1)) *
                    Matrix.CreateRotationX(transform.value.LocalRotation.X) *
                    Matrix.CreateRotationY(transform.value.LocalRotation.Y) *
                    Matrix.CreateRotationZ(transform.value.LocalRotation.Z) *
                    Matrix.CreateBillboard(Vector3.Zero, this.camera.distance * Camera3D.ISOMETRIC_OFFSET, Camera3D.ISOMETRIC_UP, Camera3D.ISOMETRIC_OFFSET) *
                    Matrix.CreateTranslation(skeleton.info.translation + transform.value.Translation);
                ((BasicEffect)this.skeletonRenderer.Effect).World = m;

                this.skeletonRenderer.Draw(skeleton.value);
                this.skeletonRenderer.End();
            }

            this.skeletonList.Clear();
        }

        List<Entity> healthList = new List<Entity>();
        public void DrawHealth(Time time)
        {
            var v = this.camera.View;
            var p = this.camera.Projection;
            this.effect.View = v;
            this.effect.Projection = p;
            this.effect.TextureEnabled = true;
            this.effect.Texture = this.healthBar_G;

            foreach (var entity in this.healthList)
            {
                ref HealthComponent healtComponent = ref entity.Get<HealthComponent>();
                ref Transform3DComponent transform = ref entity.Get<Transform3DComponent>();

                // Custom model matrix to Billboard the skeleton to the screen so that its not flat
                var m =
                    Matrix.CreateScale(transform.value.Scale * new Vector3(-1, 1, 1)) * Matrix.CreateScale(1.5f, 0.3f, 1f) *
                    Matrix.CreateRotationX(transform.value.LocalRotation.X) *
                    Matrix.CreateRotationY(transform.value.LocalRotation.Y) *
                    //Matrix.CreateRotationZ(transform.value.LocalRotation.Z) * // Removed to clamp health bars to fixed rotation
                    Matrix.CreateBillboard(Vector3.Zero, this.camera.distance * Camera3D.ISOMETRIC_OFFSET, Camera3D.ISOMETRIC_UP, Camera3D.ISOMETRIC_OFFSET) *
                    Matrix.CreateTranslation(transform.value.Translation) * Matrix.CreateTranslation(0, 0, 6f);

                this.effect.World = Matrix.CreateScale(healtComponent.Health / healtComponent.TotalHealth, 1, 1) * m;

                this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
                this.graphicsDevice.Indices = this.indexBuffer;

                this.graphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (var pass in this.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
                }
            }

            this.healthList.Clear();
        }



        public void Update(Time time)
        {
            var bounds = this.camera.ViewBounds;

            // Add small buffer to bounds
            bounds.LowerBound -= 10 * Vector2.One;
            bounds.UpperBound += 10 * Vector2.One;

            this.quadtree.QueryAABB((element) =>
            {
                var entity = element.Value;
                if (entity.Has<ModelComponent>() &&
                    entity.Has<Transform3DComponent>()
                    )
                {
                    this.modelList.Add(entity);
                }

                if (entity.Has<SpineSkeletonComponent>() &&
                    entity.Has<Transform3DComponent>() &&
                    entity.Has<WorldSpaceComponent>()
                    )
                {
                    this.skeletonList.Add(entity);
                }

                if (entity.Has<HealthComponent>() &&
                    entity.Has<Transform3DComponent>()
                    )
                {
                    this.healthList.Add(entity);
                }

                return true;
            }, ref bounds);

            this.DrawModels(time);
            this.DrawSpineSkeletons(time);
            this.DrawHealth(time);
        }

        public void Dispose()
        {

        }
        
    }
}

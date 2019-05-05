using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DefaultEcs;
using DefaultEcs.System;

using tainicom.Aether.Animation;

using Hazmat.Components;
using Hazmat.Utilities;
using Hazmat.PostProcessor;

namespace Hazmat.Systems
{
    class ModelAnimationUpdateSystem : AEntitySystem<Time>
    {
        public ModelAnimationUpdateSystem(World world) : base(
            world.GetEntities()
            .With<ModelComponent>()
            .With<ModelAnimationComponent>()
            .Build()
            )
        { }


        protected override void PreUpdate(Time state)
        {
            Hazmat.Instance.ActiveState.GetInstance<RenderCapture>().Begin();
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var model = ref entity.Get<ModelComponent>();
            ref var animation = ref entity.Get<ModelAnimationComponent>();

            animation.animations.Update(state.DeltaSpan, true, Matrix.Identity);

            foreach (var mesh in model.value.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.UpdateVertices(animation.animations.AnimationTransforms);
                }
            }
        }
    }
}

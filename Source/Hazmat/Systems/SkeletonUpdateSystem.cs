﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.Components;
using Hazmat.Graphics;
using Hazmat.Utilities;

namespace Hazmat.Systems
{
    class SkeletonUpdateSystem : AEntitySystem<Time>
    {

        public SkeletonUpdateSystem(World world) : base(
            world.GetEntities()
            .With<SpineSkeletonComponent>()
            .With<Transform2DComponent>()
            .Build()
            )
        {
        }

        protected override void Update(Time state, in Entity entity)
        {
            ref var skeleton = ref entity.Get<SpineSkeletonComponent>();
            ref var transform = ref entity.Get<Transform2DComponent>();

            var translation = Camera2D.WorldToPerspective(transform.value.Translation + skeleton.info.translation);
            var scale = transform.value.Scale * skeleton.info.scale;

            skeleton.value.X = translation.X;
            skeleton.value.Y = translation.Y;
            skeleton.value.ScaleX = scale.X;
            skeleton.value.ScaleY = scale.Y;
            skeleton.value.UpdateWorldTransform();
        }

    }
}
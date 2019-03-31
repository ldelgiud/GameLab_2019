using System;
using System.Diagnostics;

using DefaultEcs;

using Meltdown.Components;

namespace Meltdown.Utilities.Extensions
{
    public static class EntityExtensions
    {
        public static void Delete(this Entity entity)
        {
            if (entity.Has<AABBComponent>())
            {
                ref AABBComponent aabb = ref entity.Get<AABBComponent>();
                aabb.quadtree.RemoveNode(aabb.element);
            }

            entity.Dispose();
        }
    }
}

using Meltdown.Graphics;

namespace Meltdown.Components
{
    struct WorldTransformComponent
    {
        public Transform value;

        public WorldTransformComponent(Transform transform)
        {
            this.value = transform;
        }
    }
}

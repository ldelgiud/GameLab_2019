using Hazmat.Graphics;

namespace Hazmat.Components
{
    struct Transform2DComponent
    {
        public Transform2D value;

        public Transform2DComponent(Transform2D transform)
        {
            this.value = transform;
        }
    }
}

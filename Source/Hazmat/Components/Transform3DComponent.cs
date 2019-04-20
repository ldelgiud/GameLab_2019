using Hazmat.Graphics;

namespace Hazmat.Components
{
    struct Transform3DComponent
    {
        public Transform3D value;

        public Transform3DComponent(Transform3D transform)
        {
            this.value = transform;
        }
    }
}

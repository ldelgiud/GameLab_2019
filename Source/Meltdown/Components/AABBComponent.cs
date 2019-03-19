using DefaultEcs;

using tainicom.Aether.Physics2D.Collision;

namespace Meltdown.Components
{
    struct AABBComponent
    {
        public AABB aabb;
        public Element<Entity> element;

        public AABBComponent(AABB aabb, Element<Entity> element)
        {
            this.aabb = aabb;
            this.element = element;
        }
    }
}

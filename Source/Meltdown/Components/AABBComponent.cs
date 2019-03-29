using DefaultEcs;

using tainicom.Aether.Physics2D.Collision;

namespace Meltdown.Components
{
    public struct AABBComponent
    {
        public QuadTree<Entity> quadtree;
        public bool solid;
        public AABB aabb;
        public Element<Entity> element;

        public AABBComponent(QuadTree<Entity> quadtree, AABB aabb, Element<Entity> element, bool solid)
        {
            this.quadtree = quadtree;
            this.aabb = aabb;
            this.element = element;
            this.solid = solid;
        }
    }
}

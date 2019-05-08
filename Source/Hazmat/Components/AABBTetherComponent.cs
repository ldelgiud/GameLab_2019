using System;

using DefaultEcs;



namespace Hazmat.Components
{
    struct AABBTetherComponent
    {
        public Entity parent;

        public AABBTetherComponent(Entity parent)
        {
            this.parent = parent;
        }
    }
}

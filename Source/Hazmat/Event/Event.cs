using System;

using DefaultEcs;

namespace Hazmat.Event
{
    public abstract class Event
    {
        public virtual void Initialize(World world, Entity entity) { }

        public abstract void Update(World world);
    }
}

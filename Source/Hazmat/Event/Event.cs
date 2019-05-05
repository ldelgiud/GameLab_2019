using System;

using DefaultEcs;

using Hazmat.Utilities;

namespace Hazmat.Event
{
    public abstract class Event
    {
        public virtual void Initialize(World world, Entity entity) { }

        public abstract void Update(Time time, World world);
    }
}

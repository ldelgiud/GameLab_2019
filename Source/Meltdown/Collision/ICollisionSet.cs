using DefaultEcs;

namespace Meltdown.Collision
{
    /// <summary>
    /// Interface to define classes that can accept collision events
    /// </summary>
    interface ICollisionSet
    {
        void AddCollision(Entity collider, Entity collidee);
    }

    /// <summary>
    /// NOP collision set that discards collision events
    /// </summary>
    class NOPCollisionSet : ICollisionSet
    {
        public void AddCollision(Entity collider, Entity collidee) { }
    }
}

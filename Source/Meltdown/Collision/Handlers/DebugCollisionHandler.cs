using System.Diagnostics;
using DefaultEcs;

namespace Meltdown.Collision.Handlers
{
    /// <summary>
    /// Debug collision handler that prints the collision type and participants
    /// </summary>
    class DebugCollisionHandler : CollisionHandler
    {
        public DebugCollisionHandler(World world) : base(world.GetEntities().Build(), world.GetEntities().Build()) { }

        public override void HandleCollision(CollisionType type, Entity collider, Entity collidee)
        {
           // Debug.WriteLine(string.Format("Collision of type {0} between entity {1} and entity {2}", type.ToNameString(), collider, collidee));
        }
    }
}

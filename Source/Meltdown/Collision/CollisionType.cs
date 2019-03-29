using System;

namespace Meltdown.Collision
{
    enum CollisionType
    {
        Start,
        Ongoing,
        Stop,
    }

    static class CollisionTypeExtensions
    {
        public static String ToNameString(this CollisionType type)
        {
            switch(type)
            {
                case CollisionType.Start:
                    return "Start";
                case CollisionType.Ongoing:
                    return "Ongoing";
                case CollisionType.Stop:
                    return "Stop";
                default:
                    throw new Exception();
            }
        }
    }
}

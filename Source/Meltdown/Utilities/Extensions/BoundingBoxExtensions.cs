using Microsoft.Xna.Framework;

namespace Meltdown.Utilities.Extensions
{
    public static class BoundingBoxExtensions
    {
        public static float Width(this BoundingBox boundingBox)
        {
            return boundingBox.Max.X - boundingBox.Min.X;
        }

        public static float Height(this BoundingBox boundingBox)
        {
            return boundingBox.Max.Y - boundingBox.Min.Y;
        }

        public static float Depth(this BoundingBox boundingBox)
        {
            return boundingBox.Max.Z - boundingBox.Min.Z;
        }
    }
}

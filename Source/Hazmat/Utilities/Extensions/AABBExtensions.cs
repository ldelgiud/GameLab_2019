using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Collision;

namespace Hazmat.Utilities.Extensions
{
    public static class AABBExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aabb"></param>
        /// <param name="rotation">0 no rotation, 1 rotate 90 deg etc.</param>
        /// <returns></returns>
        public static AABB rotate(this AABB aabb, int rotation) 
        {
            switch (rotation)
            {
                case 0:
                    return aabb;
                case 1:
                    return new AABB(aabb.Center,aabb.Height, aabb.Width);
                case 2:
                    return aabb;
                case 3:
                    return new AABB(aabb.Center,aabb.Height, aabb.Width);
                default:
                    return aabb;
            }
        }
    }
}

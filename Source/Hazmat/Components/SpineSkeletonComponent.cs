using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spine;

using Hazmat.Utilities;

namespace Hazmat.Components
{
    struct SpineSkeletonComponent
    {
        public Skeleton value;
        public SkeletonInfo info;

        public SpineSkeletonComponent(Skeleton skeleton, SkeletonInfo skeletonInfo)
        {
            this.value = skeleton;
            this.info = skeletonInfo;
        }
    }
}

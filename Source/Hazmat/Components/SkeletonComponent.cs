﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spine;

using Hazmat.Utilities;

namespace Hazmat.Components
{
    struct SkeletonComponent
    {
        public Skeleton value;
        public SkeletonInfo info;

        public SkeletonComponent(Skeleton skeleton, SkeletonInfo skeletonInfo)
        {
            this.value = skeleton;
            this.info = skeletonInfo;
        }
    }
}
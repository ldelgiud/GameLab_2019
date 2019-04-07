using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spine;

namespace Meltdown.Components
{
    struct AtlasComponent
    {
        public Atlas value;

        public AtlasComponent(Atlas atlas)
        {
            this.value = atlas;
        }
    }
}

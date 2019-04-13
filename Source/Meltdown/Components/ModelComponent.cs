using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.Utilities;

namespace Meltdown.Components
{
    struct ModelComponent
    {
        public Model value;
        public ModelInfo info;

        public ModelComponent(Model model, ModelInfo info)
        {
            this.value = model;
            this.info = info;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Components
{
    struct ModelComponent
    {
        public Model value;

        public ModelComponent(Model model)
        {
            this.value = model;
        }
    }
}

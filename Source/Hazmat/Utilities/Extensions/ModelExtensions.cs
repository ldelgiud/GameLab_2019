using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meltdown.Utilities.Extensions
{
    static class ModelExtensions
    {

        public static ModelBone FindBone(this Model model, String name)
        {
            return model.Root.FindBone(name);
        }

        public static ModelBone FindBone(this ModelBone bone, String name)
        {
            if (bone.Name == name)
            {
                return bone;
            }
            else
            {
                foreach (var child in bone.Children)
                {
                    var result = child.FindBone(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

    }
}

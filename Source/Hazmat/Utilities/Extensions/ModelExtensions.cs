using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hazmat.Utilities.Extensions
{
    public static class ModelExtensions
    {

        public static Matrix ModelMatrix(this ModelBone bone)
        {
            if (bone.Parent == null)
            {
                return bone.Transform;
            }
            else
            {
                return bone.Transform * bone.Parent.ModelMatrix();
            }
        }

        public static int BoneIndex(this Model model, String name)
        {
            for (int i = 0; i < model.Bones.Count; ++i)
            {
                if (model.Bones[i].Name == name)
                {
                    return i;
                }
            }

            throw new Exception("Bone not found!");
        }

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

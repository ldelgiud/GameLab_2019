using System.Diagnostics;

using Microsoft.Xna.Framework;

using tainicom.Aether.Physics2D.Collision;

namespace Meltdown.Components
{
    public class ScreenTransformComponent
    {
        ScreenTransformComponent parent;
        Matrix translation;
        Matrix rotation;
        Matrix scaling;


        public Vector3 Translation
        {
            get
            {
                var transform = this.GlobalTransform;
                return new Vector3(transform.M14, transform.M24, transform.M34);
            }
        }

        public Vector3 Scaling
        {
            get
            {
                var transform = this.GlobalTransform;
                return new Vector3(
                    new Vector3(transform.M11, transform.M21, transform.M31).Length(),
                    new Vector3(transform.M12, transform.M22, transform.M32).Length(),
                    new Vector3(transform.M13, transform.M23, transform.M33).Length()
                    );
            }
        }

        public Matrix LocalTransform { get { return this.translation * (this.rotation * this.scaling); } }
        public Matrix GlobalTransform
        {
            get
            {
                return (this.parent != null) ? this.parent.GlobalTransform * this.LocalTransform : this.LocalTransform;
            }
        }

        public ScreenTransformComponent(ScreenTransformComponent parent, Matrix translation, Matrix rotation, Matrix scaling)
        {
            this.parent = parent;
            this.translation = translation;
            this.rotation = rotation;
            this.scaling = scaling;
        }

        public void Translate(float x, float y, float z)
        {
            this.translation *= Matrix.Transpose(Matrix.CreateTranslation(x, y, z));
        }

        public void Scale(float scale)
        {
            this.scaling *= Matrix.CreateScale(scale);
        }

        public void Scale(float x, float y, float z)
        {
            this.scaling *= Matrix.CreateScale(x, y, z);
        } 
    }
}

using System.Diagnostics;

using Microsoft.Xna.Framework;

using Meltdown.Utilities.Extensions;

namespace Meltdown.Graphics
{
    class Transform
    {
        public static Transform LookAt(Vector3 position, Vector3 target, Vector3 up)
        {
            Matrix matrix = Matrix.Transpose(Matrix.CreateLookAt(position, target, up));
            return new Transform(matrix.Translation(), matrix.Rotation(), matrix.Scale());
        }

        Transform parent;

        bool dirty = true;

        public Vector3 position { get; private set; }
        public Vector3 rotation { get; private set; }
        public Vector3 scale { get; private set; }

        public Matrix Translation
        {
            get
            {
                return Matrix.Transpose(Matrix.CreateTranslation(this.position));
            }
        }

        public Matrix RotationX
        {
            get
            {
                return Matrix.Transpose(Matrix.CreateRotationX(this.rotation.X));
            }
        }

        public Matrix RotationY
        {
            get
            {
                return Matrix.Transpose(Matrix.CreateRotationY(this.rotation.Y));
            }
        }

        public Matrix RotationZ
        {
            get
            {
                return Matrix.Transpose(Matrix.CreateRotationZ(this.rotation.Z));
            }
        }

        public Matrix Scaling
        {
            get
            {
                return Matrix.CreateScale(this.scale);
            }
        }

        Matrix transform;

        public Matrix LocalTransform
        {
            get
            {
                if (this.dirty)
                {
                    this.transform = this.Translation * this.RotationZ * this.RotationY * this.RotationX * this.Scaling;
                    this.dirty = false;
                }
                return this.transform;
            }
        }

        public Matrix GlobalTransform
        {
            get
            {
                return this.parent == null ? this.LocalTransform : this.parent.GlobalTransform * this.LocalTransform;
            }
        }

        public Transform(Vector3? position = null, Vector3? rotation = null, Vector3? scale = null, Transform parent = null)
        {
            this.position = position ?? Vector3.Zero;
            this.rotation = rotation ?? Vector3.Zero;
            this.scale = scale ?? Vector3.One;
            this.parent = parent;
        }

        public void TranslateX(float x) => Translate(new Vector3(x, 0, 0));
        public void TranslateY(float y) => Translate(new Vector3(0, y, 0));
        public void TranslateZ(float z) => Translate(new Vector3(0, 0, z));
        public void Translate(float x, float y, float z) => Translate(new Vector3(x, y, z));

        public void Translate(Vector3 vector)
        {
            this.position += vector;
            this.dirty = true;
        }

        public void SetPositionX(float x) => SetPosition(new Vector3(x, this.position.Y, this.position.Z));
        public void SetPositionY(float y) => SetPosition(new Vector3(this.position.X, y, this.position.Z));
        public void SetPositionZ(float z) => SetPosition(new Vector3(this.position.X, this.position.Y, z));
        public void SetPosition(float x, float y, float z) => SetPosition(new Vector3(x, y, z));

        public void SetPosition(Vector3 vector)
        {
            this.position = vector;
            this.dirty = true;
        }

        public void RotateX(float radians) => Rotate(new Vector3(radians, 0, 0));
        public void RotateY(float radians) => Rotate(new Vector3(0, radians, 0));
        public void RotateZ(float radians) => Rotate(new Vector3(0, 0, radians));
        public void Rotate(float x, float y, float z) => Rotate(new Vector3(x, y, z));

        public void Rotate(Vector3 vector)
        {
            this.rotation += vector;
            this.dirty = true;
        }

        public void SetRotationX(float radians) => SetRotation(new Vector3(radians, this.rotation.Y, this.rotation.Z));
        public void SetRotationY(float radians) => SetRotation(new Vector3(this.rotation.X, radians, this.rotation.Z));
        public void SetRotationZ(float radians) => SetRotation(new Vector3(this.rotation.X, this.rotation.Y, radians));
        public void SetRotation(float x, float y, float z) => SetRotation(new Vector3(x, y, z));

        public void SetRotation(Vector3 vector)
        {
            this.rotation = vector;
            this.dirty = true;
        }

        public void ScaleX(float x) => Scale(new Vector3(x, 0, 0));
        public void ScaleY(float y) => Scale(new Vector3(0, y, 0));
        public void ScaleZ(float z) => Scale(new Vector3(0, 0, z));
        public void Scale(float x, float y, float z) => Scale(new Vector3(x, y, z));

        public void Scale(Vector3 vector)
        {
            this.scale *= vector;
            this.dirty = true;
        }

        public void SetScaleX(float x) => SetScale(new Vector3(x, this.scale.Y, this.scale.Z));
        public void SetScaleY(float y) => SetScale(new Vector3(this.scale.X, y, this.scale.Z));
        public void SetScaleZ(float z) => SetScale(new Vector3(this.scale.X, this.scale.Y, z));
        public void SetScale(float x, float y, float z) => SetScale(new Vector3(x, y, z));

        public void SetScale(Vector3 vector)
        {
            this.scale = vector;
            this.dirty = true;
        }
    }
}

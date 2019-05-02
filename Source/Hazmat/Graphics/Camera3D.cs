using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hazmat.Graphics;

namespace Hazmat.Graphics
{
    class Camera3D
    {
        public static Vector3 ISOMETRIC_OFFSET = new Vector3(-1, -1, 1);
        public static Vector3 ISOMETRIC_UP = new Vector3(1, 1, 1);

        public Transform3D Transform { get; set; }
        public float distance;

        public float width;
        public float height;

        public Vector3 Translation
        {
            get
            {
                return this.Transform.Translation * new Vector3(1, 1, 0) + distance * Camera3D.ISOMETRIC_OFFSET;
            }
        }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(this.Translation, this.Transform.Translation * new Vector3(1, 1, 0), ISOMETRIC_UP);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreateOrthographic(width, height, 0, this.height + this.distance * MathF.Sqrt(2) + 0.1f);
            }
        }

        public Camera3D(Transform3D transform, float distance, float width, float height)
        {
            this.distance = distance;
            this.Transform = transform;
            this.width = width;
            this.height = height;
        }

    }
}

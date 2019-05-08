using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using tainicom.Aether.Physics2D.Collision;

using Hazmat.Graphics;
using Hazmat.Utilities;
using Hazmat.Utilities.Extensions;

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

        public AABB ViewBounds
        {
            get
            {
                var transformMatrix =
                    Matrix.CreateRotationX(MathF.PI / 4) *
                    Matrix.CreateRotationZ(-MathF.PI / 4) *
                    Matrix.CreateTranslation(this.Translation);
                var topLeft = Vector3.Transform(new Vector3(-this.width / 2, this.height / 2, 0), transformMatrix) + this.Translation;
                var topRight = Vector3.Transform(new Vector3(this.width / 2, this.height / 2, 0), transformMatrix) + this.Translation;
                var bottomLeft = Vector3.Transform(new Vector3(-this.width / 2, -this.height / 2, 0), transformMatrix) + this.Translation;
                var bottomRight = Vector3.Transform(new Vector3(this.width / 2, -this.height / 2, 0), transformMatrix) + this.Translation;

                var direction = -Camera3D.ISOMETRIC_OFFSET;
                var normal = Vector3.UnitZ;
                var planePoint = Vector3.Zero;

                var topLeftT = Vector3.Dot(normal, planePoint - topLeft) / Vector3.Dot(normal, direction);
                var topRightT = Vector3.Dot(normal, planePoint - topRight) / Vector3.Dot(normal, direction);
                var bottomLeftT = Vector3.Dot(normal, planePoint - bottomLeft) / Vector3.Dot(normal, direction);
                var bottomRightT = Vector3.Dot(normal, planePoint - bottomRight) / Vector3.Dot(normal, direction);


                var topLeftIntersection = topLeft + direction * topLeftT;
                var topRightIntersection = topRight + direction * topRightT;
                var bottomLeftIntersection = bottomLeft + direction * bottomLeftT;
                var bottomRightIntersection = bottomRight + direction * bottomRightT;

                var width = topRightIntersection.X - bottomLeftIntersection.X;
                var height = topLeftIntersection.Y - bottomRightIntersection.Y;

                return new AABB(this.Transform.Translation.ToVector2(), width, height);
            }
        }

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
                return Matrix.CreateOrthographic(width, height, -10f, this.height + this.distance * MathF.Sqrt(2) + 10f);
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

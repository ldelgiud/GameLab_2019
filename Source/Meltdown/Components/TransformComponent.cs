using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Meltdown.Utilities;
using System.Diagnostics;


namespace Meltdown.Components
{
    class TransformComponent
    {

        //___________________________________
        //              Fields
        //___________________________________
        private TransformComponent parentTransform;
        /// <summary>
        /// Parent transform. If null the parent is the world.
        /// </summary>
        public TransformComponent ParentTransform
        {
            get { return parentTransform; }
            set
            {
                SetParent(value);
            }
        }

        private List<TransformComponent> childTransforms;
        /// <summary>
        /// Children of this transform.
        /// </summary>
        public List<TransformComponent> ChildTransforms
        {
            get { return new List<TransformComponent>(childTransforms); }
            private set { }
        }

        /// <summary>
        /// Position relative to the parent.
        /// </summary>
        private Vector2 localPosition;
        public Vector2 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = value;
                //foreach(TransformComponent child in childTransforms)
                //{
                //    child.LocalPosition = localPosition + child.localPosition;
                //    Debug.WriteLine("Child " + child.LocalPosition);
                //}
                SetDirty();
            }
        }

        /// <summary>
        /// Rotation relative to the parent.
        /// </summary>
        private float localRotation;
        public float LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Scale relative to the parent.
        /// </summary>
        private Vector2 localScale;
        public Vector2 LocalScale
        {
            get { return localScale; }
            set
            {
                localScale = value;
                SetDirty();
            }
        }


        /// <summary>
        /// Is localToWorldTransform out of date.
        /// </summary>
        private bool isDirty = false;

        /// <summary>
        /// Transform from local coordinates to world coordinates.
        /// </summary>
        private Matrix localToWorldMatrix;


        /// <summary>
        /// Is worldToLocalTransform out of date.
        /// </summary>
        private bool isInverseDirty = false;

        /// <summary>
        /// Transform from world coordinates to local coordinates.
        /// </summary>
        private Matrix worldToLocalMatrix;




        //___________________________________
        //          Private Methods
        //___________________________________
        /// <summary>
        /// Whenever the local values gets updated this method needs to be called.
        /// The 
        /// </summary>
        private void SetDirty()
        {
            if (!isDirty)
            {
                isDirty = true;
                isInverseDirty = true;

                // also children are dirty
                foreach (TransformComponent child in childTransforms)
                {
                    child.SetDirty();
                }
            }
        }



        //___________________________________
        //          Public Methods
        //___________________________________
        /// <summary>
        /// Set the parent transform.
        /// </summary>
        public void SetParent(TransformComponent parent)
        {
            // Remove this object from child of old parent
            if (parentTransform != null)
            {
                parentTransform.childTransforms.Remove(this);
            }

            // Add new parent
            parentTransform = parent;

            // Add this object as child of new parent
            if (parent != null) parentTransform.childTransforms.Add(this);

            SetDirty();
        }

        /// <summary>
        /// Basic transformation matrix calculated with local vlaues.
        /// Used for other transformations.
        /// </summary>
        /// <returns></returns>
        public Matrix CalculateLocalToParentMatrix()
        {
            //Debug.WriteLine("Local Position: " + localPosition + ". Translation Matrix: " + Matrix.CreateTranslation(localPosition.X, localPosition.Y, 0));
            Matrix temp = Matrix.Transpose(Matrix.CreateTranslation(localPosition.X, localPosition.Y, 0) *
                   Matrix.CreateRotationZ(localRotation) *
                   Matrix.CreateScale(localScale.X, localScale.Y, 0));
            //Debug.WriteLine("Local Position: " + localPosition + ". Translation Matrix: " + temp);

            return temp;
        }

        /// <summary>
        /// Computes the matrix that is used to transform points from local to world coordinate frame.
        /// It is computed recurisvely.
        /// </summary>
        /// <returns></returns>
        public Matrix GetLocalToWorldMatrix()
        {
            if (isDirty)
            {
                if (ParentTransform == null)
                {
                    localToWorldMatrix = CalculateLocalToParentMatrix();
                }
                else
                {
                    //Debug.WriteLine("Local Position CHILD: " + localPosition + ", ParentMatrix: " + ParentTransform.GetLocalToWorldMatrix());
                    localToWorldMatrix = ParentTransform.GetLocalToWorldMatrix() * CalculateLocalToParentMatrix();
                }
                isDirty = false;
            }
            //Debug.WriteLine("Local Position: " + localPosition + ". GetLocalToWorldMatrix: " + localToWorldMatrix);
            return localToWorldMatrix;
        }

        /// <summary>
        /// Returns matrix used to transform points from world to local coordinate frame.
        /// </summary>
        public Matrix GetWorldToLocalMatrix()
        {
            if (isInverseDirty)
            {
                worldToLocalMatrix = Matrix.Invert(GetLocalToWorldMatrix());
                isInverseDirty = false;
            }
            return worldToLocalMatrix;
        }

        /// <summary>
        /// Transforms a point from local to world coordinate space.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 TransformPoint(Vector2 point)
        {
            Vector4 transformedHomo3DPoint = MathUtil.MultiplyMatrixVector(GetLocalToWorldMatrix(), new Vector4(point.X, point.Y, 0,1));
            return new Vector2(transformedHomo3DPoint.X, transformedHomo3DPoint.Y);
        }

        /// <summary>
        /// Transform a point from world to local coordinate frame.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 InverseTransformPoint(Vector2 point)
        {
            Vector4 transformedHomo3DPoint = MathUtil.MultiplyMatrixVector(GetWorldToLocalMatrix(), new Vector4(point.X, point.Y, 0, 1));
            return new Vector2(transformedHomo3DPoint.X, transformedHomo3DPoint.Y);
        }

        //___________________________________
        //           Constructors
        //___________________________________
        /// <summary>
        /// Simple constructor. Parent is world.
        /// </summary>
        public TransformComponent(Vector2 localPosition)
        {
            this.localPosition = localPosition;
            this.localRotation = 0.0f;
            this.localScale = Vector2.One;

            childTransforms = new List<TransformComponent>();
            parentTransform = null;
            localToWorldMatrix = Matrix.Identity;
            worldToLocalMatrix = Matrix.Identity;
            isDirty = false;
            isInverseDirty = false;
        }

        /// <summary>
        /// Constructor with a given parent.
        /// </summary>
        public TransformComponent(TransformComponent parent, Vector2 localPosition)
        {
            this.localPosition = localPosition;
            this.localRotation = 0.0f;
            this.localScale = Vector2.One;

            childTransforms = new List<TransformComponent>();
            SetParent(parent);
            localToWorldMatrix = Matrix.Identity;
            worldToLocalMatrix = Matrix.Identity;
            isDirty = false;
            isInverseDirty = false;
        }
    }
}

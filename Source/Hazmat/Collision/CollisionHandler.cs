using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;


using DefaultEcs;

using Hazmat.Components;
using Hazmat.Utilities.Extensions;

namespace Hazmat.Collision
{
    /// <summary>
    /// Base class for collision handling.
    /// </summary>
    abstract class CollisionHandler
    {
        public readonly bool commutative;
        public readonly Type[] colliderTypes;
        public readonly Type[] collideeTypes;

        public CollisionHandler(Type[] colliderTypes, Type[] collideeTypes, bool commutative = false)
        {
            this.colliderTypes = colliderTypes;
            this.collideeTypes = collideeTypes;
            this.commutative = commutative;
        }

        public abstract void HandleCollision(CollisionType type, Entity collider, Entity collidee);
    }
}

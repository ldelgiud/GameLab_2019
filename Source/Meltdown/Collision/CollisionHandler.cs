using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;


using DefaultEcs;

using Meltdown.Components;
using Meltdown.Utilities.Extensions;

namespace Meltdown.Collision
{
    /// <summary>
    /// Base class for collision handling.
    /// </summary>
    abstract class CollisionHandler
    {
        bool commutative;
        Type[] colliderTypes;
        Type[] collideeTypes;

        public CollisionHandler(Type[] colliderTypes, Type[] collideeTypes, bool commutative = false)
        {
            this.colliderTypes = colliderTypes;
            this.collideeTypes = collideeTypes;
            this.commutative = commutative;
        }

        public void HandleCollisions(CollisionType type, IEnumerable<(Entity, Entity)> entities)
        {

            foreach (var tuple in entities)
            {
                var (collider, collidee) = tuple;
                var colliderMatches = this.colliderTypes.All(component => collider.Has(component));
                var collideeMatches = this.collideeTypes.All(component => collidee.Has(component));

                if (colliderMatches && collideeMatches)
                {
                    this.HandleCollision(type, collider, collidee);
                }

                if (this.commutative)
                {
                    colliderMatches = this.collideeTypes.All(component => collider.Has(component));
                    collideeMatches = this.colliderTypes.All(component => collidee.Has(component));

                    if (colliderMatches && collideeMatches)
                    {
                        this.HandleCollision(type, collidee, collider);
                    }
                }
            }
            
        }

        public abstract void HandleCollision(CollisionType type, Entity collider, Entity collidee);
    }
}

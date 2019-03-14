using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Entities;

namespace Meltdown.Components
{
    class BoundingBoxComponent : ICollisionActor
    {
        public IShapeF Bounds { get; set; }
        public Entity entity;
        
        public BoundingBoxComponent(IShapeF bounds)
        {
            this.Bounds = bounds;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}

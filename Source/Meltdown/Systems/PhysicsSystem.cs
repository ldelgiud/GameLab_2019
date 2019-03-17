using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Meltdown.Components;
using Microsoft.Xna.Framework;

using Nez;
namespace Meltdown.Systems
{
    class PhysicsSystem : EntityProcessingSystem
    {

        public PhysicsSystem(Matcher matcher) : base( matcher )
        {

        }

        public override void process(Entity entity)
        {
            var velocity = entity.getComponent<VelocityComponent>();

            // Add velocity to position, scaled to meters per second
            CollisionResult collisionResult;

            entity.getComponent<Collider>().collidesWithAny(
                ref velocity.velocity, 
                out collisionResult);

            entity.position += velocity.velocity;


        }
    }
}

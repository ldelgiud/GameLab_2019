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
            var position = entity.getComponent<PositionComponent>();
            var velocity = entity.getComponent<VelocityComponent>();

            BoundingBoxComponent boundingBox = this.collisionMapper.Get(id);
            // Add velocity to position, scaled to meters per second
            Vector2 newPos = position.position;
            newPos.X += velocity.velocity.X * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f) ;
            newPos.Y += velocity.velocity.Y * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f) ;

                
            quadtree.Remove(new QuadtreeData(boundingBox));
            boundingBox.Bounds.Position = newPos;
            List<QuadtreeData> collisions = quadtree.Query(boundingBox.Bounds);
            if (collisions.Count == 0)
            {
                position.position = newPos;
            } else
            {
                boundingBox.Bounds.Position = position.position;
            }
            quadtree.Insert(new QuadtreeData(boundingBox));
            
        }
    }
}

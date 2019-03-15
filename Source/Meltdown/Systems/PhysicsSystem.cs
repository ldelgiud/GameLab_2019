using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Collisions;

using Meltdown.Components;
using Microsoft.Xna.Framework;

namespace Meltdown.Systems
{
    class PhysicsSystem : EntityUpdateSystem
    {
        Quadtree quadtree;
        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<VelocityComponent> velocityMapper;
        ComponentMapper<BoundingBoxComponent> collisionMapper;


        public PhysicsSystem(Quadtree quadtree) : base(Aspect.All(
            typeof(PositionComponent),
            typeof(VelocityComponent),
            typeof(BoundingBoxComponent)))
        {
            this.quadtree = quadtree;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
            this.velocityMapper = mapperService.GetMapper<VelocityComponent>();
            this.collisionMapper = mapperService.GetMapper<BoundingBoxComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int id in this.ActiveEntities)
            {
                PositionComponent position = this.positionMapper.Get(id);
                VelocityComponent velocity = this.velocityMapper.Get(id);
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
}

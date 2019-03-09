using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Components;
using Microsoft.Xna.Framework;

namespace Meltdown.Systems
{
    class PhysicsSystem : EntityUpdateSystem
    {
        // Quadtree quadtree;

        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<VelocityComponent> velocityMapper;

        public PhysicsSystem() : base(Aspect.All(typeof(PositionComponent), typeof(VelocityComponent)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            this.positionMapper = mapperService.GetMapper<PositionComponent>();
            this.velocityMapper = mapperService.GetMapper<VelocityComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int id in this.ActiveEntities)
            {
                PositionComponent position = this.positionMapper.Get(id);
                VelocityComponent velocity = this.velocityMapper.Get(id);

                // Add velocity to position, scaled to meters per second
                position.position.X += velocity.velocity.X * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f) ;
                position.position.Y += velocity.velocity.Y * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f) ;
            }
        }
    }
}

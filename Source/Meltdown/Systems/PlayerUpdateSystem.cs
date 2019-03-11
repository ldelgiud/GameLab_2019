using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using Meltdown.Components;
using Microsoft.Xna.Framework.Input;

namespace Meltdown.Systems
{
    class PlayerUpdateSystem : EntityUpdateSystem
    {

        ComponentMapper<PositionComponent> positionMapper;
        ComponentMapper<VelocityComponent> velocityMapper;

        public PlayerUpdateSystem() : base(Aspect.All(
            typeof(PlayerComponent), 
            typeof(PositionComponent),
            typeof(VelocityComponent)
            ))
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
                VelocityComponent velocity = this.velocityMapper.Get(id);
                KeyboardState state = Keyboard.GetState();
                bool A = state.IsKeyDown(Keys.A);
                bool W = state.IsKeyDown(Keys.W);
                bool S = state.IsKeyDown(Keys.S);
                bool D = state.IsKeyDown(Keys.D);

                if (A) { velocity.velocity = new Vector2(-80, 0); }
                else if (W) { velocity.velocity = new Vector2( 0,  -80); }
                else if (S) { velocity.velocity = new Vector2( 0,  80); }
                else if (D) { velocity.velocity = new Vector2( 80, 0); }
                else { velocity.velocity = new Vector2(0, 0); }
                
            }
        }
    }
}

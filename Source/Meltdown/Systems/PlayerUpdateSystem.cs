using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using Meltdown.Components;
using Microsoft.Xna.Framework.Input;

using Nez;

namespace Meltdown.Systems
{
    class PlayerUpdateSystem : EntityProcessingSystem
    {
        
        public PlayerUpdateSystem(Matcher matcher) : base( matcher )
        {

        }

        public override void process(Entity entity)
        {

            var velocity = entity.getComponent<VelocityComponent>();
            KeyboardState state = Keyboard.GetState();
            bool A = state.IsKeyDown(Keys.A);
            bool W = state.IsKeyDown(Keys.W);
            bool S = state.IsKeyDown(Keys.S);
            bool D = state.IsKeyDown(Keys.D);

            Vector2 newVelocity = new Vector2(0, 0);
            if (A) { newVelocity.X += -80; }
            if (W) { newVelocity.Y += -80; }
            if (S) { newVelocity.Y += 80; }
            if (D) { newVelocity.X += 80; }
            if (!(A||W||S||D)){ velocity.velocity = new Vector2(0, 0); }

            velocity.velocity = newVelocity;
        }
    }
}

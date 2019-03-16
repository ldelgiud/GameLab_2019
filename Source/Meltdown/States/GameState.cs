using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.State;
using Meltdown.Game_Elements;
using Meltdown.Systems;

namespace Meltdown.States
{
    class GameState : IState
    {
        Game1 game;
        SpriteBatch spriteBatch;
        //public World world;
        //public Quadtree quadtree;


        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            this.world.Draw(gameTime);
        }

        public void Initialize(Game game)
        {
           

           
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public IStateTransition Update(GameTime gameTime)
        {
            this.world.Update(gameTime);
            return null;
        }
    }
}

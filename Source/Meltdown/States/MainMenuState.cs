using Meltdown.State;
using Microsoft.Xna.Framework;

namespace Meltdown.States
{
    public class MainMenuState : IState
    {

        public void Initialize(Game game)
        {
        }

        public void Resume()
        {
        }

        public void Suspend()
        {
        }

        public void Destroy()
        {
        }

        public IStateTransition Update(GameTime gameTime)
        {
            return new PushStateTransition(new GameState());
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}

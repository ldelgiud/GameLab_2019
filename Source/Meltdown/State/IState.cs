using Microsoft.Xna.Framework;

namespace Meltdown.States
{
    public interface IState
    {
        /// <summary>
        /// Hook for when the state is first created.
        /// </summary>
        /// <param name="game"></param>
        void Initialize(Game game);

        /// <summary>
        /// Hook for when the state is resumed.
        /// </summary>
        void Resume();

        /// <summary>
        /// Hook for when the state is suspended.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Hook for when the state is destroyed.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Update call for state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Next state</returns>
        IStateTransition Update(GameTime gameTime);

        /// <summary>
        /// Draw call for state
        /// </summary>
        /// <param name="gameTime"></param>
        void Draw(GameTime gameTime);
    }
}

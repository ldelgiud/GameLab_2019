using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Meltdown.State
{
    public abstract class State
    {
        protected Dictionary<Type, Object> data = new Dictionary<Type, Object>();

        public T GetInstance<T>()
        {
            return (T)this.data[typeof(T)];
        }

        /// <summary>
        /// Hook for when the state is first created.
        /// </summary>
        /// <param name="game"></param>
        public virtual void Initialize(Game game) { }

        /// <summary>
        /// Hook for when the state is resumed.
        /// </summary>
        public virtual void Resume(object data) { }

        /// <summary>
        /// Hook for when the state is suspended.
        /// </summary>
        public virtual void Suspend() { }

        /// <summary>
        /// Hook for when the state is destroyed.
        /// </summary>
        public virtual void Destroy() { }

        /// <summary>
        /// Update call for state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Next state</returns>
        public abstract IStateTransition Update(GameTime gameTime);

        /// <summary>
        /// Draw call for state
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);
    }
}

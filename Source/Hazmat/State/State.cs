using System;
using System.Collections.Generic;

using Hazmat.Utilities;

namespace Hazmat.State
{
    public abstract class State
    {
        public IStateTransition stateTransition;

        protected Dictionary<Type, Object> data = new Dictionary<Type, Object>();

        protected void SetInstance<T>(T instance)
        {
            data.Add(typeof(T), instance);
        }

        public T GetInstance<T>()
        {
            return (T)this.data[typeof(T)];
        }

        /// <summary>
        /// Hook for when the state is first created.
        /// </summary>
        /// <param name="game"></param>
        public virtual void Initialize(Hazmat game) { }

        /// <summary>
        /// Hook for when the state is resumed.
        /// </summary>
        public virtual void Resume(object data) {
            this.stateTransition = null;
        }

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
        /// <param name="time"></param>
        /// <returns>Next state</returns>
        public virtual IStateTransition Update(Time time)
        {
            return this.stateTransition;
        }

        /// <summary>
        /// Draw call for state
        /// </summary>
        /// <param name="time"></param>
        public abstract void Draw(Time time);
    }
}

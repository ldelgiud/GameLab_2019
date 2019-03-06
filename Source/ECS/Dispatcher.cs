using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Class to store and dispatch systems.
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// Set of systems
        /// </summary>
        HashSet<ECS.SystemBase> systems = new HashSet<SystemBase>();

        /// <summary>
        /// Register 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        public void Register<S>()
            where S : ECS.SystemBase, new()
        {
            this.systems.Add(new S());
        }

        /// <summary>
        /// Run all systems on the given context and return the actions generated.
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <returns>Actions generated</returns>
        public ActionStore Tick(Context ctx)
        {
            ActionStore actionStore = new ActionStore();
            foreach (SystemBase system in this.systems)
            {
                system.Tick(actionStore, ctx);
            }

            return actionStore;
        }

    }
}
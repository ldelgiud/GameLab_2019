using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// Storage class for actions.
    /// </summary>
    public class ActionStore
    {
        /// <summary>
        /// List of queued up actions.
        /// </summary>
        List<IAction> actions = new List<IAction>();

        /// <summary>
        /// Add an action to the queue.
        /// </summary>
        /// <param name="reducer">Action</param>
        public void Add(IAction reducer)
        {
            this.actions.Add(reducer);
        }

        /// <summary>
        /// Applies all actions to the given context.
        /// </summary>
        /// <param name="ctx">Context</param>
        public void Apply(Context ctx)
        {

            this.actions.ForEach(action => {Console.WriteLine(action); action.Apply(ctx);});
            this.actions.Clear();
        }
    }
}
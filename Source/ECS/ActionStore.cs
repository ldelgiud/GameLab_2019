using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ECS
{
    /// <summary>
    /// Storage class for actions.
    /// </summary>
    public class ActionStore
    {
        /// <summary>
        /// Queued up actions, indexed by action type.
        /// </summary>
        /// 
        Dictionary<Type, ConcurrentQueue<IAction>> actions = new Dictionary<Type, ConcurrentQueue<IAction>>();

        public ActionStore(IEnumerable<Type> actionTypes)
        {
            foreach (var type in actionTypes)
            {
                this.actions.Add(type, new ConcurrentQueue<IAction>());
            }
        }

        /// <summary>
        /// Add an action to the queue.
        /// </summary>
        /// <param name="action">Action</param>
        public void Add(IAction action)
        {
            // TODO: Add debug error if type not registered
            this.actions[action.GetType()].Enqueue(action);
        }

        /// <summary>
        /// Applies all actions to the given context.
        /// </summary>
        /// <param name="ctx">Context</param>
        public void Apply(Context ctx)
        {
            // TODO: Sort actions by some priority
            foreach (var actions in this.actions.Values)
            {
                IAction action;
                while (actions.TryDequeue(out action))
                {
                    action.Apply(ctx);
                }
            }
        }
    }
}
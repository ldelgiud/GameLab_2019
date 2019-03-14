using System;

namespace Meltdown.State
{
    /// <summary>
    /// Interface for state transitions.
    /// </summary>
    public interface IStateTransition {}

    /// <summary>
    /// Indicates that the specified state should be pushed on to the state stack.
    /// </summary>
    public class PushStateTransition : IStateTransition
    {
        /// <summary>
        /// The state to be pushed
        /// </summary>
        public IState State;

        public PushStateTransition(IState state)
        {
            this.State = state;
        }
    }

    /// <summary>
    /// Indicates that the specified state should replace the current state.
    /// </summary>
    public class SwapTransition : IStateTransition
    {
        /// <summary>
        /// The state to be swaped
        /// </summary>
        public IState State;

        public SwapTransition(IState state)
        {
            this.State = state;
        }
    }

    /// <summary>
    /// Indicates that the stack should remove the current state.
    /// </summary>
    public class PopStateTransition : IStateTransition
    {
        public Object Data;

        public PopStateTransition(Object data)
        {
            this.Data = data;
        }
    }

    /// <summary>
    /// Indicates that the program should exit.
    /// </summary>
    public class ExitTransition : IStateTransition
    {
    }
}

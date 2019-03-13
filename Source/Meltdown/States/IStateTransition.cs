namespace Meltdown.States
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
        public State State;

        public PushStateTransition(State state)
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
        public State State;

        public SwapTransition(State state)
        {
            this.State = state;
        }
    }

    /// <summary>
    /// Indicates that the stack should remove the current state.
    /// </summary>
    public class PopStateTransition : IStateTransition
    {
    }

    /// <summary>
    /// Indicates that the program should exit.
    /// </summary>
    public class ExitTransition : IStateTransition
    {
    }
}

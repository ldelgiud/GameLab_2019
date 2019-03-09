namespace ECS
{
    /// <summary>
    /// Interface for actions
    /// </summary>
    public interface IAction {
        /// <summary>
        /// Method stub for applying an action to a context
        /// </summary>
        /// <param name="ctx"></param>
        void Apply(Context ctx);
    }
}
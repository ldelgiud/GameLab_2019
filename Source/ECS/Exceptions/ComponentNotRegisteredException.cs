using System;

namespace ECS.Exceptions
{
    /// <summary>
    /// Exception to indicate that a component wasn't registered before use.
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public class ComponentNotRegisteredException<C> : Exception
        where C: struct
    {
        public ComponentNotRegisteredException() : base(typeof(C).ToString()) { }
        public ComponentNotRegisteredException(Exception inner) : base(typeof(C).ToString(), inner) { }
    }

    public class ComponentNotRegisteredException : Exception
    {
        public ComponentNotRegisteredException(Type type) : base(type.ToString()) { }
        public ComponentNotRegisteredException(Exception inner, Type type) : base(type.ToString(), inner) { }
    }
}
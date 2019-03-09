using System;

namespace ECS
{
    /// <summary>
    /// Attribute to annotate IComponentRef
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ComponentRefAttribute : Attribute
    {
        public ComponentRefType Type { get; set; }

        public ComponentRefAttribute(ComponentRefType type)
        {
            this.Type = type;
        }

        public static ComponentRefType GetType(Type type)
        {
            return ((ComponentRefAttribute)type.GetCustomAttributes(false)[0]).Type;
        }
    }

    /// <summary>
    /// Concrete type for ICompomentRef
    /// </summary>
    public enum ComponentRefType
    {
        Concrete,
        Optional,
        Not
    }

    /// <summary>
    /// Interface for component references.
    /// </summary>
    public interface IComponentRef
    {
        /// <summary>
        /// Auxiliary constructor for component references.
        /// </summary>
        /// <typeparam name="T">Reference Type</typeparam>
        /// <param name="ctx">Context</param>
        /// <param name="entity">Entity</param>
        /// <returns>Instance of component reference</returns>
        IComponentRef With(Context ctx, Entity entity);
    }

    /// <summary>
    /// Reference to a component.
    /// </summary>
    /// <typeparam name="C">Component type</typeparam>
    [ComponentRef(ComponentRefType.Concrete)]
    public struct ConcreteRef<C> : IComponentRef
        where C : struct
    {
        /// <summary>
        /// Context that is referred to.
        /// </summary>
        Context ctx;

        /// <summary>
        /// Entity that is referred in the given Context.
        /// </summary>
        Entity entity;

        /// <summary>
        /// Auxiliary constructor to set context and entity.
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="entity">Entity</param>
        /// <returns>This with context and entity set</returns>
        public IComponentRef With(Context ctx, Entity entity)
        {
            this.ctx = ctx;
            this.entity = entity;

            return this;
        }

        /// <summary>
        /// The referenced value.
        /// </summary>
        public ref C Value
        {
            get
            {
                return ref this.ctx.GetComponent<C>(this.entity);
            }
        }
    }

    /// <summary>
    /// Reference to a component that may or may not exist.
    /// </summary>
    /// <typeparam name="C">Component type</typeparam>
    [ComponentRef(ComponentRefType.Optional)]
    public struct OptionalRef<C> : IComponentRef
        where C : struct
    {
        /// <summary>
        /// Context that is referred to.
        /// </summary>
        Context ctx;

        /// <summary>
        /// Entity that is referred in the given Context.
        /// </summary>
        Entity entity;

        /// <summary>
        /// Auxiliary constructor to set context and entity.
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="entity">Entity</param>
        /// <returns>This with context and entity set</returns>
        public IComponentRef With(Context ctx, Entity entity)
        {
            this.ctx = ctx;
            this.entity = entity;

            return this;
        }

        /// <summary>
        /// If the referenced value exists.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return this.ctx.HasComponent<C>(this.entity);
            }
        }

        /// <summary>
        /// The referenced value.
        /// </summary>
        public ref C Value
        {
            get
            {
                return ref this.ctx.GetComponent<C>(this.entity);
            }
        }
    }

    /// <summary>
    /// Marker type to indicate that a component type should not exist.
    /// </summary>
    /// <typeparam name="C">Component type</typeparam>
    [ComponentRef(ComponentRefType.Not)]
    public struct NotRef<C> : IComponentRef
        where C : struct
    {
        public IComponentRef With(Context ctx, Entity entity)
        {
            return this;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS
{
    /// <summary>
    /// Base class for all systems.
    /// </summary>
    public abstract class SystemBase
    {
        /// <summary>
        /// First type parameter of the system. Must be a <see cref="ConcreteRef{C}"/>.
        /// </summary>
        Type firstType;

        /// <summary>
        /// Remaining type parameters of the system.
        /// </summary>
        (ComponentRefType, Type)[] types;

        /// <summary>
        /// List of types of possible actions this system generates.
        /// </summary>
        Type[] actionTypes;

        public SystemBase(Type firstRefType, Type[] refTypes, Type[] actionTypes)
        {

            // Assert first type is a ConcreteRef
            if (ComponentRefAttribute.GetType(firstRefType) != ComponentRefType.Concrete)
            {
                throw new Exception("First type is not a concrete ref type!");
            }

            // Extract the component type
            this.firstType = firstRefType.GetGenericArguments()[0];

            // Map the remaining values to component types and ref types
            this.types = refTypes.Select(refType => {
                return (
                    ComponentRefAttribute.GetType(refType),
                    refType.GetGenericArguments()[0]
                );
            }).ToArray();

            // Add action types
            this.actionTypes = actionTypes;
        }

        /// <summary>
        /// Get the list of action types this system generates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetActionTypes()
        {
            return this.actionTypes;
        }

        public abstract void Tick(ActionStore actionStore, Context ctx, Entity entity);

        public void Tick(ActionStore actionStore, Context ctx)
        {
            // Get bitset of the first type (must exist because first type is a non-unit storage concrete ref)
            Bitset bitset = ctx.GetBitset(this.firstType).Clone();

            // Merge bitsets of other components
            foreach ((ComponentRefType, Type) pair in this.types)
            {
                ComponentRefType refType = pair.Item1;
                Type type = pair.Item2;

                IStorage storage = ctx.GetStorage(type);

                switch (refType)
                {
                    // Required component
                    case ComponentRefType.Concrete:
                        switch (storage.BitsetType)
                        {
                            // Empty bitset, no entities match
                            case BitsetType.None:
                                return;
                            // Some bitset, some entities match
                            case BitsetType.Some:
                                bitset.IAnd(storage.Bitset);
                                break;
                            // Full bitset, all entities match
                            case BitsetType.All:
                                break;
                        }
                        break;
                    // Optional component, skip
                    case ComponentRefType.Optional:
                        break;
                    // Required not component
                    case ComponentRefType.Not:
                        switch (storage.BitsetType)
                        {
                            // Empty bitset, all entities match
                            case BitsetType.None:
                                break;
                            // Some bitset, some entities match
                            case BitsetType.Some:
                                bitset.IAndNot(storage.Bitset);
                                break;
                            // Full bitset, no entities match
                            case BitsetType.All:
                                return;

                        }
                        break;
                }
            }

            ctx.GetEnumerator(bitset).AsParallel().ForAll(entity => this.Tick(actionStore, ctx, entity));
        }
    }

    public abstract class System<A> : SystemBase
    where A : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity, 
                (A)new A().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a);
    }

    public abstract class System<A, B> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b);
    }

    public abstract class System<A, B, C> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c);
    }

    public abstract class System<A, B, C, D> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
        where D : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
            typeof(D),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity),
                (D)new D().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c, D d);
    }

    public abstract class System<A, B, C, D, E> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
        where D : IComponentRef, new()
        where E : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
            typeof(D),
            typeof(E),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity),
                (D)new D().With(ctx, entity),
                (E)new E().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c, D d, E e);
    }

    public abstract class System<A, B, C, D, E, F> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
        where D : IComponentRef, new()
        where E : IComponentRef, new()
        where F : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
            typeof(D),
            typeof(E),
            typeof(F),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity),
                (D)new D().With(ctx, entity),
                (E)new E().With(ctx, entity),
                (F)new F().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c, D d, E e, F f);
    }

    public abstract class System<A, B, C, D, E, F, G> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
        where D : IComponentRef, new()
        where E : IComponentRef, new()
        where F : IComponentRef, new()
        where G : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
            typeof(D),
            typeof(E),
            typeof(F),
            typeof(G),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity),
                (D)new D().With(ctx, entity),
                (E)new E().With(ctx, entity),
                (F)new F().With(ctx, entity),
                (G)new G().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c, D d, E e, F f, G g);
    }

    public abstract class System<A, B, C, D, E, F, G, H> : SystemBase
        where A : IComponentRef, new()
        where B : IComponentRef, new()
        where C : IComponentRef, new()
        where D : IComponentRef, new()
        where E : IComponentRef, new()
        where F : IComponentRef, new()
        where G : IComponentRef, new()
        where H : IComponentRef, new()
    {
        public System(Type[] actionTypes) : base(typeof(A), new Type[] {
            typeof(B),
            typeof(C),
            typeof(D),
            typeof(E),
            typeof(F),
            typeof(G),
            typeof(H),
        }, actionTypes) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity),
                (C)new C().With(ctx, entity),
                (D)new D().With(ctx, entity),
                (E)new E().With(ctx, entity),
                (F)new F().With(ctx, entity),
                (G)new G().With(ctx, entity),
                (H)new H().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b, C c, D d, E e, F f, G g, H h);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS
{
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

        HashSet<Type> dependencies = new HashSet<Type>();

        public SystemBase(Type firstRefType, Type[] refTypes)
        {

            // Assert first type is a ConcreteRef
            if (ComponentRefAttribute.GetType(firstRefType) != ComponentRefType.Concrete)
            {
                throw new Exception("First type is not a ref type!");
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
        }

        public SystemBase(Type firstRefType, Type[] refTypes, Type[] dependencies) : this(firstRefType, refTypes)
        {
            this.dependencies.UnionWith(dependencies);
        }

        public abstract void Tick(ActionStore actionStore, Context ctx, Entity entity);

        public void Tick(ActionStore actionStore, Context ctx)
        {
            Bitset bitset = ctx.GetBitset(this.firstType).Clone();

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

            foreach (Entity entity in ctx.GetEnumerator(bitset))
            {
                this.Tick(actionStore, ctx, entity);
            }
        }
    }

    public abstract class System<A> : SystemBase
    where A : IComponentRef, new()
    {
        public System() : base(typeof(A), new Type[] { }) { }

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
        public System() : base(typeof(A), new Type[] { typeof(B) }) { }

        public sealed override void Tick(ActionStore actionStore, Context ctx, Entity entity)
        {
            this.Tick(actionStore, entity,
                (A)new A().With(ctx, entity),
                (B)new B().With(ctx, entity)
            );
        }

        public abstract void Tick(ActionStore actionStore, Entity entity, A a, B b);
    }
}
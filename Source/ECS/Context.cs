using System;
using System.Collections.Generic;
using System.Linq;

using Bitset = CRoaring.RoaringBitmap;

using ECS.Storages;
using ECS.Exceptions;

namespace ECS
{
    /// <summary>
    /// The <c>Context</c> class holds the game state.
    /// It maintains an <see cref="IStorage"/> for each registered component and permits access through the use of <see cref="Entity"/> handles.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// List of current entities.
        /// </summary>
        // TODO: Convert to expanding array.
        private List<Entity> entities = new List<Entity>();

        /// <summary>
        /// List of free entity indices.
        /// </summary>
        private HashSet<UInt32> freeList = new HashSet<UInt32>();

        /// <summary>
        /// Storages for components.
        /// </summary>
        private Dictionary<Type, IStorage> storages = new Dictionary<Type, IStorage>();

        /// <summary>
        /// Register a component type with a given storage.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <param name="storage">Associated storage</param>
        public void Register<C>(IStorage<C> storage)
            where C : struct
        {
            this.storages[typeof(C)] = storage;
        }

        /// <summary>
        /// Retrieve the entity at a certain index.
        /// </summary>
        /// <param name="index">Index of the entity</param>
        /// <returns>Entity with the given index</returns>
        public Entity GetEntity(UInt32 index)
        {
            return this.entities[(Int32)index];
        }

        /// <summary>
        /// Add a component to a given entity.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <param name="entity">Associated entity</param>
        /// <param name="value">Component instance</param>
        public void AddComponent<C>(Entity entity, C value)
            where C : struct
        {
            this.GetStorage<C>().Add(entity.Index, value);
        }

        /// <summary>
        /// Set the component for a unit storage.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <param name="value">Component instance</param>
        public void AddUnitComponent<C>(C value)
            where C : struct
        {
            ((UnitStorage<C>)this.GetStorage<C>()).Add(0, value);
        }

        /// <summary>
        /// Check if a component has a certain component.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <param name="entity">Associated entity</param>
        /// <returns>Whether the entity has the given component</returns>
        public bool HasComponent<C>(Entity entity)
        where C: struct
        {
            throw new NotImplementedException();
            //return this.GetBitSet<C>().GetBit((Int32)entity.Index);
        }

        public bool HasComponent(Entity entity, Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve a component instance reference.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <param name="entity">Associated entity</param>
        /// <returns>Reference to the component instance</returns>
        public ref C GetComponent<C>(Entity entity)
            where C : struct
        {
            return ref this.GetStorage<C>().Get(entity.Index);
        }

        /// <summary>
        /// Retrieve a bitset associated to the given component type.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <returns>Associated bitset</returns>
        public Bitset GetBitSet<C>()
            where C : struct
        {
            IStorage storage;
            if (this.storages.TryGetValue(typeof(C), out storage))
            {
                return ((IStorage<C>)storage).Bitset;
            }
            throw new ComponentNotRegisteredException<C>();
        }

        /// <summary>
        /// Get storage for a given component type.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <returns>Associated storage instance</returns>
        public IStorage<C> GetStorage<C>()
            where C : struct
        {
            IStorage storage;
            if (this.storages.TryGetValue(typeof(C), out storage))
            {
                return (IStorage<C>)storage;
            }

            throw new ComponentNotRegisteredException<C>();
        }

       /// <summary>
       /// Get storage for a given component type
       /// </summary>
       /// <param name="type">Component type</param>
       /// <returns>Associated storage</returns>
        public IStorage GetStorage(Type type)
        {
            IStorage storage;
            if (this.storages.TryGetValue(type, out storage))
            {
                return storage;
            }

            throw new ComponentNotRegisteredException(type);
        }

        /// <summary>
        /// Allocates a new entity.
        /// </summary>
        /// <returns>Entity handle</returns>
        public Entity CreateEntity()
        {
            if (freeList.Count != 0)
            {
                // Re-use existing free slot
                UInt32 index = this.freeList.First();

                // Remove from free list
                this.freeList.Remove(index);

                // Increment generation
                Entity entity = this.entities[(Int32)index];
                entity.Generation += 1;

                // Write back entity
                this.entities[(Int32)index] = entity;
                return entity;
            }
            else
            {
                // Create new entity
                Entity entity = new Entity();

                // Set index
                entity.Index = (UInt32)this.entities.Count;


                this.entities.Add(entity);
                return entity;
            }
        }

        /// <summary>
        /// Remove entity from the context.
        /// </summary>
        /// <param name="entity">Entity</param>
        public void RemoveEntity(Entity entity)
        {
            // TODO: Implement properly
            this.freeList.Add(entity.Index);
        }

        /// <summary>
        /// Retrieve bitset associated with the given component type.
        /// </summary>
        /// <typeparam name="C">Component type</typeparam>
        /// <returns>Associated bitset</returns>
        public Bitset GetBitset<C>()
            where C: struct
        {
            return this.GetStorage<C>().Bitset;
        }

        /// <summary>
        /// Retrieve bitset associated with the given component type.
        /// </summary>
        /// <param name="type">Component type</param>
        /// <returns>Associated bitset</returns>
        public Bitset GetBitset(Type type)
        {
            return this.GetStorage(type).Bitset;
        }

        /// <summary>
        /// Converts a bitset into an enumerable over entities in the context
        /// </summary>
        /// <param name="bitset">Bitset</param>
        /// <returns>Entity enumerable</returns>
        public IEnumerable<Entity> GetEnumerator(Bitset bitset)
        {
            return bitset.Select(index => this.entities[(Int32)index]);
        }

        public override string ToString()
        {
            Int32 entityCount = this.entities.Count;
            String freeList = String.Join(", ", this.freeList.Select(i => i.ToString()));

            List<String> storages = new List<String>();
            foreach (Object obj in this.storages)
            {
                storages.Add("    " + obj.ToString());
            }

            List<String> data = new List<String>();

            foreach (Entity entity in this.entities.Where(i => !this.freeList.Contains(i.Index)))
            {
                List<String> components = new List<String>();
                foreach (KeyValuePair<Type, IStorage> kv in this.storages)
                {
                    Type type = kv.Key;
                    IStorage storage = kv.Value;

                    if (storage.Contains(entity.Index))
                    {
                        components.Add(storage.GetString(entity.Index));
                    }
                }

                data.Add(String.Format("{0}: {{ {1} }}", entity.Index, String.Join(", ", components)));
            }

            return String.Format(
@"Context {{
    Total Entities: {0:d},
    Data:
    {2}
}}", entityCount,
    freeList,
    String.Join("\n    ", data)
    );
        }
    }
}
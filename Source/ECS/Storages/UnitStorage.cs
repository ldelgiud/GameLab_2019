using System;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS.Storages
{

    /// <summary>
    /// Storage class for unit types. Only one of this component may exist in the context and will match like a normal component.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class UnitStorage<T> : IStorage<T>
        where T : struct
    {
        public BitsetType BitsetType
        {
            get
            {
                return (this.valid) ? BitsetType.All : BitsetType.None;
            }
        }

        public Bitset Bitset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool valid = false;
        T value;

        public void Add(uint index, T value)
        {
            this.valid = true;
            this.value = value;
        }

        public ref T Get(uint index)
        {
            return ref this.value;
        }

        public bool Contains(uint index)
        {
            return this.valid;
        }

        public void Remove(uint index)
        {
            this.valid = false;
            this.value = new T();
        }

        public String GetString(uint index)
        {
            return this.value.ToString();
        }
    }
}
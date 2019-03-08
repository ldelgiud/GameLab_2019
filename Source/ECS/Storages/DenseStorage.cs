using System;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS.Storages
{
    /// <summary>
    /// Storage class for dense components. Maps directly to an underlying array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DenseStorage<T> : IStorage<T>
        where T: struct
    {
        DynamicArray<T> data = new DynamicArray<T>(DynamicArray<T>.INITIAL_ARRAY_SIZE);

        public BitsetType BitsetType
        {
            get
            {
                return BitsetType.Some;
            }
        }

        public Bitset Bitset { get; private set; } = new Bitset();

        public void Add(uint index, T value)
        {
            // Add element to bitset
            this.Bitset.Add(index);

            // Add element to array
            this.data.Add(index, value);
        }

        public ref T Get(uint index)
        {
            return ref this.data.Get(index);
        }

        public bool Contains(uint index)
        {
            return this.Bitset.Contains(index);
        }

        public void Remove(uint index)
        {
            // Remove element from array
            this.data.Remove(index);
        }

        public String GetString(uint index)
        {
            return this.data.Get(index).ToString();
        }
    }
}

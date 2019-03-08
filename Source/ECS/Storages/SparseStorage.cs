using System;
using System.Collections;
using System.Collections.Generic;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS.Storages
{
    /// <summary>
    /// Storage class for sparse components.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class SparseStorage<T> : IStorage<T>
        where T : struct
    {
        Dictionary<uint, uint> indices = new Dictionary<uint, uint>();

        uint length = 0;
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
            this.data.Add(this.length, value);

            // Map index to element
            this.indices.Add(index, this.length);

            // Increment length
            ++this.length;
        }

        public ref T Get(uint index)
        {
            return ref this.data.Get(this.indices[index]);
        }

        public bool Contains(uint index)
        {
            return this.Bitset.Contains(index);
        }

        public void Remove(uint index)
        {
            // Set target element to last element
            this.data.Add(index, this.data.Get(this.length - 1));

            // Clear last element
            this.data.Remove(this.length - 1);

            // Update index
            this.indices[this.length - 1] = index;

            // Decrement length;
            --this.length;
        }

        public String GetString(uint index)
        {
            return this.data.Get(this.indices[index]).ToString();
        }
    }
}
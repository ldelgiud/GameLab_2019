using System;
using System.Collections;
using System.Collections.Generic;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS.Storages
{
    /// <summary>
    /// Storage class for sparse components
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class SparseStorage<T> : IStorage<T>
        where T : struct
    {
        Dictionary<UInt32, Int32> indices;
        Int32 length;
        T[] data;

        public BitsetType BitsetType
        {
            get
            {
                return BitsetType.Some;
            }
        }

        public Bitset Bitset { get; private set; } = new Bitset();

        public SparseStorage()
        {
            this.indices = new Dictionary<uint, int>();
            // TODO: dynamic array sizing
            this.length = 0;
            this.data = new T[1000];
        }

        public void Add(uint index, T value)
        {
            this.Bitset.Add(index);
            this.data[this.length] = value;
            this.indices.Add(index, this.length);
            this.length += 1;
        }

        public ref T Get(uint index)
        {
            return ref this.data[this.indices[index]];
        }

        public bool Contains(uint index)
        {
            return this.Bitset.Contains(index);
        }

        public void Remove(uint index)
        {
            // TODO: swap remove
            throw new NotImplementedException();
            // base.Remove(index);
            // this.indices.Remove(index);
            // this.data
        }

        public String GetString(uint index)
        {
            return this.data[this.indices[index]].ToString();
        }
    }
}
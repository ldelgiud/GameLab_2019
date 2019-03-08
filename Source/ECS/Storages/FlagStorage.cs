using System;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS.Storages
{
    /// <summary>
    /// Storage class for marking entities. Does not hold any data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlagStorage<T> : IStorage<T>
        where T: struct
    {
        public class FlagStorageException : Exception
        {
            public FlagStorageException(String msg) : base(msg) { }
        }

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
            // Add to bitset
            this.Bitset.Add(index);
        }

        public ref T Get(uint index)
        {
            throw new FlagStorageException("Attempt to get flag storage data!");
        }

        public bool Contains(uint index)
        {
            return this.Bitset.Contains(index);
        }

        public void Remove(uint index)
        {
            // Remove from bitset
            this.Bitset.Remove(index);
        }

        public String GetString(uint index)
        {
            return typeof(T).Name;
        }

    }
}

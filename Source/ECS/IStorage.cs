using System;

using Bitset = CRoaring.RoaringBitmap;

namespace ECS
{
    /// <summary>
    /// Component storage interface.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Accessor to simplified bitset state.
        /// </summary>
        BitsetType BitsetType { get; }

        /// <summary>
        /// Accessor to bitset.
        /// </summary>
        Bitset Bitset { get; }

        /// <summary>
        /// Check if storage contains an index.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Whether the item exists</returns>
        bool Contains(uint index);

        String GetString(uint index);
    }

    /// <summary>
    /// Enum to represent the state of a bitset. Used to simplify full/empty bitset operations.
    /// </summary>
    public enum BitsetType
    {
        None,
        Some,
        All
    }

    /// <summary>
    /// Generic component storage interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStorage<T> : IStorage
        where T : struct
    {
        /// <summary>
        /// Add value to the storage
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value</param>
        void Add(uint index, T value);

        /// <summary>
        /// Get reference to value inside storage with given index.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Reference to value</returns>
        ref T Get(uint index);

        /// <summary>
        /// Remove value at index.
        /// </summary>
        /// <param name="index">Index</param>
        void Remove(uint index);
    }
}
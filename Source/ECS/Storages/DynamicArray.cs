using System;

namespace ECS.Storages
{
    /// <summary>
    /// An array that will expand as needed. Steps in powers of 2.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct DynamicArray<T>
        where T: struct
    {
        internal const uint INITIAL_ARRAY_SIZE = 16;

        T[] data;

        /// <summary>
        /// Constructor with a capacity
        /// </summary>
        /// <param name="capacity">Initial size of the array</param>
        public DynamicArray(uint capacity)
        {
            this.data = new T[capacity];
        }

        /// <summary>
        /// Calculates the next power of 2 greater than the given number.
        /// </summary>
        /// <param name="num">Input number</param>
        /// <returns>Next largest power of 2</returns>
        private uint RoundToNextPowerOf2(uint num)
        {
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return num + 1;
        }

        /// <summary>
        /// Set an element in the array.
        /// </summary>
        /// <param name="index">Index to set</param>
        /// <param name="value">Value to set</param>
        public void Add(uint index, T value)
        {
            if (index >= this.data.Length)
            {
                T[] newData = new T[this.RoundToNextPowerOf2(index)];
                Array.Copy(this.data, newData, data.Length);
                this.data = newData;
            }

            this.data[index] = value;
        }

        /// <summary>
        /// Get reference to an element in the array.
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>Reference to the element</returns>
        public ref T Get(uint index)
        {
            return ref this.data[index];
        }

        /// <summary>
        /// Zero an element in the array.
        /// </summary>
        /// <param name="index">Index to zero</param>
        public void Remove(uint index)
        {
            this.data[index] = new T();
        }


    }
}

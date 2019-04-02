using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meltdown.Pathfinding
{
    class MinHeap<T> where T : IHeapItem<T>
    {
        List<T> items;
        int currentItemCount;
        public int Count { get
            {
                return currentItemCount;
            }
        }

        public MinHeap()
        {
            items = new List<T>();
        }

        public void UpdateItem(T item)
        {
            UpSort(item);
        }

        public void Add(T item)
        {
            item.HeapIndex = this.currentItemCount;
            items.Add(item);

            UpSort(item);
            this.currentItemCount++;
        }

        public T PopMin()
        {
            --currentItemCount;
            T min = items[0];
            Swap(items[0], items[currentItemCount]);
           
            //TODO: check if there is a better way considering that this is good for memory
            //but bad for performances
            items.RemoveAt(currentItemCount);
            DownSort(items[0]);
            return min;
        }

        public bool Contains (T item)
        {
            return Equals(items[item.HeapIndex], item);
        }
        void DownSort(T item)
        {
            while (true)
            {
                int leftChildIndex = item.HeapIndex * 2 + 1;
                int rightChildIndex = item.HeapIndex * 2 + 2;
                int swapIndex = 0;
                if (leftChildIndex < this.currentItemCount)
                {
                    swapIndex = leftChildIndex;
                    if (rightChildIndex < this.currentItemCount)
                    {
                        if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                        {
                            swapIndex = rightChildIndex;
                        }
                    }
                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else return;
                }
                else return;
            }
        }

        void UpSort(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while(item.HeapIndex != 0)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else break;
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        void Swap(T a, T b)
        {
            items[a.HeapIndex] = b;
            items[b.HeapIndex] = a;

            int temp = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = temp;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }


    }
}

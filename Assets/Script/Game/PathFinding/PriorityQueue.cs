using System;
using System.Collections.Generic;

namespace lzengine
{
    public sealed class PriorityQueue<TItem, TPriority> where TPriority : IComparable<TPriority>
    {
        private KeyValuePair<TPriority, TItem>[] _arr;
        private int _count;
        private bool _minSorted;

        public KeyValuePair<TPriority, TItem>[] Queue => _arr;
        public bool IsMinSorted => _minSorted;
        public int Count => _count;

        /// <summary>
        /// Set true if the collection is a fixed sized, or false if not.
        /// </summary>
        public bool IsFixedSize { get; private set; }

        /// <summary>
        /// A priority queue that queues items in order of priority.
        /// </summary>
        /// <param name="size">The starting size of the priority queue. Default is 8.</param>
        /// <param name="isMinPriority">If true the priority queue sorts the lowest value as having highest priority.</param>
        /// <param name="fixedSize">If true the priority queue has a fixed size so no new elements can be added.</param>
        public PriorityQueue(int size = 8, bool isMinPriority = true, bool fixedSize = false)
        {
            _arr = new KeyValuePair<TPriority, TItem>[size];
            IsFixedSize = fixedSize;
            _count = 0;
            _minSorted = isMinPriority;
        }
        public void SetDynamicResize(bool fixedSize) => IsFixedSize = fixedSize;
        private void Resize() => Array.Resize(ref _arr, _arr.Length * 2);
        /// <summary>
        /// Try insert new element in to the queue, if the queue is
        /// fixed size and is full, the new element will not be added.
        /// </summary>
        public void Insert(TItem obj, TPriority priority)
        {
            if (_count == _arr.Length)
            {
                if (IsFixedSize)
                    throw new Exception("Queue is full and cannot accept more items. The queue is set to be a fixed size of " + _arr.Length + "! Use SetDynamicResize() to enable or disable dynamic resize or create a Queue with sufficient capacity in the constructor.");
                else
                    Resize();
            }

            var kvp = new KeyValuePair<TPriority, TItem>(priority, obj);
            _arr[_count] = kvp;

            SortUp(_count);

            _count++;
        }
        /// <summary>
        /// Try get the next element in the queue.
        /// </summary>
        public bool TryPop(out TItem item, out TPriority priority)
        {
            item = default;
            priority = default;

            if (_count == 0)
                return false;

            item = _arr[0].Value;
            priority = _arr[0].Key;

            //pop the top item
            _count--;

            Swap(0, _count);// swap first and last
            SortDown(_count);

            return true;
        }
        /// <summary>
        /// Get the element at the top of the queue.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<TPriority, TItem> Top() => _arr[0];
        /// <summary>
        /// Get the element last in the queue.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<TPriority, TItem> Bottom() => _arr[^1];

        /// <summary>
        /// Clear contents of priority queue.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_arr, 0, _count);
            _count = 0;
        }

        private void Swap(int i, int i2)
        {
            var temp = _arr[i];
            _arr[i] = _arr[i2];
            _arr[i2] = temp;
        }

        private void SortDown(int count, int index = 0)
        {
            while (true)
            {
                int leftChild = ChildIndex(index);
                int rightChild = ChildIndex(index) + 1;
                if (leftChild < count)
                {
                    int swapIndex = leftChild;
                    if (rightChild < count && IsHigherPriority(_arr[rightChild], _arr[leftChild]))
                        swapIndex = rightChild;

                    if (IsHigherPriority(_arr[swapIndex], _arr[index]))
                    {
                        Swap(index, swapIndex);
                        index = swapIndex;
                    }
                    else
                        return;
                }
                else
                    return;
            }
        }
        private void SortUp(int i)
        {
            while (true)
            {
                int p = Parent(i);
                if (IsHigherPriority(_arr[i], _arr[p]))
                {
                    Swap(i, p);
                    i = p;
                }
                else
                    break;
            }
        }
        private bool IsHigherPriority(KeyValuePair<TPriority, TItem> a, KeyValuePair<TPriority, TItem> item)
        {
            int b = a.Key.CompareTo(item.Key);

            return b != 0 && ((b < 0 && _minSorted) || (b > 0 && !_minSorted));
        }
        private int Parent(int i) => (i - 1) / 2;
        private int ChildIndex(int i) => (2 * i) + 1;
    }
}

using System;
using System.Collections.Generic;

namespace _LinkedList
{
    public class _LinkedList<T>
    {
        // This LinkedList is a doubly-Linked circular list.
        internal _LinkedListNode<T> head;
        internal int count;
        internal int version;

        public _LinkedList()
        {
        }

        public _LinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        public int Count
        {
            get { return count; }
        }

        public _LinkedListNode<T> First
        {
            get { return head; }
        }

        public _LinkedListNode<T> Last
        {
            get { return head == null ? null : head.prev; }
        }

        public _LinkedListNode<T> AddAfter(_LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            _LinkedListNode<T> result = new _LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node.next, result);
            return result;
        }

        public void AddAfter(_LinkedListNode<T> node, _LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }

        public _LinkedListNode<T> AddBefore(_LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            _LinkedListNode<T> result = new _LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node, result);
            if (node == head)
            {
                head = result;
            }
            return result;
        }

        public void AddBefore(_LinkedListNode<T> node, _LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node == head)
            {
                head = newNode;
            }
        }

        public _LinkedListNode<T> AddFirst(T value)
        {
            _LinkedListNode<T> result = new _LinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
                head = result;
            }
            return result;
        }

        public void AddFirst(_LinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
                head = node;
            }
            node.list = this;
        }

        public _LinkedListNode<T> AddLast(T value)
        {
            _LinkedListNode<T> result = new _LinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
            }
            return result;
        }

        public void AddLast(_LinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
            }
            node.list = this;
        }

        public void Clear()
        {
            _LinkedListNode<T> current = head;
            while (current != null)
            {
                _LinkedListNode<T> temp = current;
                current = current.Next;   // use Next the instead of "next", otherwise it will loop forever
                temp.Invalidate();
            }

            head = null;
            count = 0;
            version++;
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException();
            }

            _LinkedListNode<T> node = head;
            if (node != null)
            {
                do
                {
                    array[index++] = node.item;
                    node = node.next;
                } while (node != head);
            }
        }

        public _LinkedListNode<T> Find(T value)
        {
            _LinkedListNode<T> node = head;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
            }
            return null;
        }

        public _LinkedListNode<T> FindLast(T value)
        {
            if (head == null) return null;

            _LinkedListNode<T> last = head.prev;
            _LinkedListNode<T> node = last;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }

                        node = node.prev;
                    } while (node != last);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.prev;
                    } while (node != last);
                }
            }
            return null;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Remove(T value)
        {
            _LinkedListNode<T> node = Find(value);
            if (node != null)
            {
                InternalRemoveNode(node);
                return true;
            }
            return false;
        }

        public void Remove(_LinkedListNode<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }

        public void RemoveFirst()
        {
            if (head == null) { throw new InvalidOperationException(); }
            InternalRemoveNode(head);
        }

        public void RemoveLast()
        {
            if (head == null) { throw new InvalidOperationException(); }
            InternalRemoveNode(head.prev);
        }

        private void InternalInsertNodeBefore(_LinkedListNode<T> node, _LinkedListNode<T> newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            version++;
            count++;
        }

        private void InternalInsertNodeToEmptyList(_LinkedListNode<T> newNode)
        {
            newNode.next = newNode;
            newNode.prev = newNode;
            head = newNode;
            version++;
            count++;
        }

        internal void InternalRemoveNode(_LinkedListNode<T> node)
        {
            if (node.next == node)
            {
                head = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (head == node)
                {
                    head = node.next;
                }
            }
            node.Invalidate();
            count--;
            version++;
        }

        internal void ValidateNewNode(_LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != null)
            {
                throw new InvalidOperationException();
            }
        }


        internal void ValidateNode(_LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != this)
            {
                throw new InvalidOperationException();
            }
        }

        public struct Enumerator
        {
            private _LinkedList<T> list;
            private _LinkedListNode<T> node;
            private int version;
            private T current;
            private int index;

            internal Enumerator(_LinkedList<T> list)
            {
                this.list = list;
                version = list.version;
                node = list.head;
                current = default(T);
                index = 0;
            }

            public T Current
            {
                get { return current; }
            }

            public bool MoveNext()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException();
                }

                if (node == null)
                {
                    index = list.Count + 1;
                    return false;
                }

                ++index;
                current = node.item;
                node = node.next;
                if (node == list.head)
                {
                    node = null;
                }
                return true;
            }
        }
    }

    public sealed class _LinkedListNode<T>
    {
        internal _LinkedList<T> list;
        internal _LinkedListNode<T> next;
        internal _LinkedListNode<T> prev;
        internal T item;
        public _LinkedListNode(T value)
        {
            this.item = value;
        }
        internal _LinkedListNode(_LinkedList<T> list, T value)
        {
            this.list = list;
            this.item = value;
        }
        public _LinkedList<T> List
        {
            get { return list; }
        }
        public _LinkedListNode<T> Next
        {
            get { return next == null || next == list.head ? null : next; }
        }
        public _LinkedListNode<T> Previous
        {
            get { return prev == null || this == list.head ? null : prev; }
        }
        public T Value
        {
            get { return item; }
            set { item = value; }
        }
        internal void Invalidate()
        {
            list = null;
            next = null;
            prev = null;
        }
    }
}

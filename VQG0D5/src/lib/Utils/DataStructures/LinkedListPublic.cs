using System;
using System.Collections;
using System.Collections.Generic;
using VQG0D5.Activity;
using VQG0D5.Exceptions;

namespace VQG0D5.Utils.DataStructures {
  public partial class LinkedList<T, K, I> : IEnumerable<T> where K : IComparable {
    public class Node {
      public T Value { get; set; }
      public K Key { get; set; }
      public I Identifier { get; set; }
      public Node Next { get; set; }
    }

    public LinkedList(bool ascending = true) {
      this.__ascending = ascending;
      this.__count = 0;
    }

    public Node Head => this.__head;
    public int Count => this.__count;

    /// <summary>
    /// Adds an item to the list in order
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <exception cref="AlreadyExistsException"></exception>
    public void Add(T data, K key, I identifier) {
      Node node = new Node() { Value = data, Key = key, Identifier = identifier, Next = null };
      Node p = this.__head;
      Node e = null;

      while (p != null && this.__IsKeyLesserOrGreater(p, key)) {
        e = p;
        p = p.Next;
      }

      if (p != null && p.Identifier.Equals(identifier)) {
        throw new AlreadyExistsException("Received data already exists in the linked list!", data);
      }

      this.__count += 1;

      if (e == null) {
        node.Next = this.__head;
        this.__head = node;
        return;
      }

      node.Next = p;
      e.Next = node;
    }

    /// <summary>
    /// Removes an item with the specified identifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <exception cref="NotFoundException"></exception>
    public void Remove(I identifier) {
      this.__Remove(
        node => !node.Identifier.Equals(identifier),
        new NotFoundException("No data found with received identifier!", identifier)
      );
    }

    /// <summary>
    /// Removes every element
    /// </summary>
    public void RemoveAll() {
      this.__head = null;
    }

    /// <summary>
    /// Determines if the list contains an item with the specified indentifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public bool Contains(I identifier) {
      Node p = this.__head;

      while (p != null) {
        if (p.Identifier.Equals(identifier)) {
          return true;
        }

        p = p.Next;
      }

      return false;
    }

    /// <summary>
    /// Filters through an ordered linked list
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public LinkedList<T, K, I> Filter(Func<T, bool> callback) {
      LinkedList<T, K, I> result = new LinkedList<T, K, I>();
      Node p = this.__head;

      while (p != null) {
        if (callback(p.Value)) {
          result.Add(p.Value, p.Key, p.Identifier);
        }

        p = p.Next;
      }

      return result;
    }

    /// <summary>
    /// Merges a linked list with this instance
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public LinkedList<T, K, I> Merge(LinkedList<T, K, I> list) {
      LinkedList<T, K, I> output = new LinkedList<T, K, I>();
      Node p1 = this.__head;
      Node p2 = list.Head;

      while (p1 != null) {
        if (!output.Contains(p1.Identifier)) {
          output.Add(p1.Value, p1.Key, p1.Identifier);
        }

        p1 = p1.Next;
      }

      while (p2 != null) {
        if (!output.Contains(p2.Identifier)) {
          output.Add(p2.Value, p2.Key, p2.Identifier);
        }

        p2 = p2.Next;
      }

      return output;
    }

    /// <summary>
    /// Gets the intersection of the specified list and this instance
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public LinkedList<T, K, I> Intersection(LinkedList<T, K, I> list) {
      LinkedList<T, K, I> output = new LinkedList<T, K, I>();
      Node p = this.__head;

      while (p != null) {
        if (list.Contains(p.Identifier)) {
          output.Add(p.Value, p.Key, p.Identifier);
        }

        p = p.Next;
      }

      return output;
    }

    /// <summary>
    /// Gets the difference of the specified list and this instance
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public LinkedList<T, K, I> Difference(LinkedList<T, K, I> list) {
      LinkedList<T, K, I> merged = this.Merge(list);
      LinkedList<T, K, I> intersection = this.Intersection(list);
      Node p = intersection.Head;

      while (p != null) {
        merged.Remove(p.Identifier);
        p = p.Next;
      }

      return merged;
    }

    /// <summary>
    /// Traverse through the list
    /// </summary>
    /// <param name="handler"></param>
    public void ForEach(TraverseHandler<T> handler) {
      Node p = this.__head;

      while (p != null) {
        handler?.Invoke(p.Value);
        p = p.Next;
      }
    }

    public IEnumerator GetEnumerator() {
      return this.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
      return new Enumerator(this.__head);
    }
  }
}

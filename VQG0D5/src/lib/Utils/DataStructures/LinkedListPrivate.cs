using System;
using System.Collections.Generic;
using VQG0D5.Exceptions;

namespace VQG0D5.Utils.DataStructures {
  public partial class LinkedList<T, K, I> : IEnumerable<T> where K : IComparable {
    protected class Enumerator : IEnumerator<T> {
      private Node __head;
      private Node __current;

      public Enumerator(Node head) {
        this.__head = head;
        this.__current = null;
      }

      public object Current => this.__current;
      T IEnumerator<T>.Current => this.__current.Value;

      public void Dispose() { }

      public bool MoveNext() {
        if (this.Current != null) {
          this.__current = this.__current.Next;
        } else {
          this.Reset();
        }

        return this.Current != null;
      }

      public void Reset() {
        this.__current = this.__head;
      }
    }

    private Node __head;
    private bool __ascending;
    private int __count;

    /// <summary>
    /// Determines if a specified key is lesser or greater based on the ordering
    /// </summary>
    /// <param name="node"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool __IsKeyLesserOrGreater(Node node, K key) {
      return this.__ascending ? node.Key.CompareTo(key) <= 0 : node.Key.CompareTo(key) >= 0;
    }

    /// <summary>
    /// Private function for remove
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="exception"></param>
    private void __Remove(Func<Node, bool> callback, CustomBaseException exception) {
      Node p = this.__head;
      Node e = null;

      while (p != null && callback(p)) {
        e = p;
        p = p.Next;
      }

      if (p == null) {
        throw exception;
      }

      this.__count -= 1;

      if (e == null) {
        this.__head = p.Next;
        return;
      }

      e.Next = p.Next;
    }
  }
}

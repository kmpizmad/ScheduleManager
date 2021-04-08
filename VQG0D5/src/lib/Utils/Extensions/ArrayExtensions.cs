using System;
using System.Collections.Generic;
using System.Linq;

namespace VQG0D5.Utils.Extensions {
  public static class ArrayExtensions {
    /// <summary>
    /// Adds a new item to the end of the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T[] Push<T>(this T[] array, T value) {
      List<T> list = array.ToList();
      list.Add(value);
      return list.ToArray();
    }

    /// <summary>
    /// Removes the last element from the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns>A tuple of the removed element and the modified array</returns>
    public static Tuple<T, T[]> Pop<T>(this T[] array) {
      var stack = new Stack<T>(array);
      var item = stack.Pop();
      return Tuple.Create(item, stack.Reverse().ToArray());
    }
  }
}

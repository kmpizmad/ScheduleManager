using System;
using System.Collections.Generic;
using System.Linq;

namespace VQG0D5.Utils.Reflection {
  public static class Conversion {
    /// <summary>
    /// Converts every item in an array to integer (if possible). Saves it to a dictionary
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="dic"></param>
    public static void ToInt(string[] arr, Dictionary<string, Tuple<object, int>> dic) {
      for (int i = 0; i < arr.Length; i++) {
        int num;
        string item = arr[i];
        bool isNumeric = int.TryParse(item, out num);

        if (isNumeric) dic[item] = Tuple.Create((object)num, i);
        else if (!dic.ContainsKey(item) || dic[item] == null) dic[item] = Tuple.Create((object)item, i);
      }
    }

    /// <summary>
    /// Converts every item in an array to DateTime (if possible). Saves it to a dictionary
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="dic"></param>
    public static void ToDate(string[] arr, Dictionary<string, Tuple<object, int>> dic) {
      for (int i = 0; i < arr.Length; i++) {
        DateTime date;
        string item = arr[i];
        bool isDate = DateTime.TryParse(item, out date);

        if (isDate) dic[item] = Tuple.Create((object)date, i);
        else if (!dic.ContainsKey(item) || dic[item] == null) dic[item] = Tuple.Create((object)item, i);
      }
    }

    /// <summary>
    /// Converts every item in an array to enum (if possible). Saves it to a dictionary
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="arr"></param>
    /// <param name="dic"></param>
    public static void ToEnum(Type enumType, string[] arr, Dictionary<string, Tuple<object, int>> dic) {
      for (int i = 0; i < arr.Length; i++) {
        string item = arr[i];
        bool isEnum = Enum.GetNames(enumType).Contains(item);

        if (isEnum) dic[item] = Tuple.Create(Enum.Parse(enumType, item), i);
        else if (!dic.ContainsKey(item) || dic[item] == null) dic[item] = Tuple.Create((object)item, i);
      }
    }

    /// <summary>
    /// Runs every conversion on each item in an array
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="arr"></param>
    /// <returns>An array of objects converted to their respective type (whenever it was possible)</returns>
    public static object[] All(Type enumType, string[] arr) {
      Dictionary<string, Tuple<object, int>> dic = new Dictionary<string, Tuple<object, int>>();

      ToInt(arr, dic);
      ToDate(arr, dic);
      ToEnum(enumType, arr, dic);

      object[] args = new object[arr.Length];

      foreach (KeyValuePair<string, Tuple<object, int>> item in dic) {
        args[item.Value.Item2] = item.Value.Item1;
      }

      return args;
    }
  }
}

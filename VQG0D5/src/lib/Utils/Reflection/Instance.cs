using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VQG0D5.Activity;
using VQG0D5.Utils.Extensions;

namespace VQG0D5.Utils.Reflection {
  public static class Instance {
    /// <summary>
    /// Gets all constructors of each subclasses of a type
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns>A dictionary where the keys represents the class name and the values are an array of constructors</returns>
    public static Dictionary<string, ConstructorInfo[]> GetConstructors(Type baseType) {
      Dictionary<string, ConstructorInfo[]> dictionary = new Dictionary<string, ConstructorInfo[]>();

      Assembly
         .GetAssembly(baseType)
         .GetTypes()
         .Where(type => !type.IsAbstract && type.IsSubclassOf(baseType))
         .ToList()
         .ForEach(type => dictionary.Add(type.Name, type.GetConstructors()));

      return dictionary;
    }

    /// <summary>
    /// Creates an instance of each class in the dictionary
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="args">Arguments for the current constructor</param>
    /// <returns>A list of instances</returns>
    public static List<object> Create(Dictionary<string, ConstructorInfo[]> dictionary, string[] args) {
      List<object> instances = new List<object>();

      foreach (KeyValuePair<string, ConstructorInfo[]> item in dictionary) {
        var (lastItem, arguments) = args.Pop();
        int interval = int.Parse(lastItem);

        string name = Enum.GetName(typeof(ActivityType), interval) + "Activity";

        if (item.Key == name) {
          foreach (ConstructorInfo ctor in item.Value) {
            object[] args_ = Conversion.All(typeof(Priority), arguments);

            if (ctor.GetParameters().Length != args_.Length) {
              continue;
            }

            instances.Add(
              ctor.Invoke(args_)
            );
          }
        }
      }

      return instances;
    }
  }
}

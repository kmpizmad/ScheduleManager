using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VQG0D5.Activity;
using VQG0D5.Exceptions;
using VQG0D5.Utils.FileSystem;
using VQG0D5.Utils.Reflection;

namespace VQG0D5 {
  public static class ClassBuilder {
    public static void Init(string path, Action<object> callback) {
      Dictionary<string, ConstructorInfo[]> constructors = Instance.GetConstructors(typeof(BaseActivity));

      Read.FromStream(path, row => {
        string[] data = row.Split(';').Where(item => item.Length > 0).ToArray();

        Instance
          .Create(constructors, data)
          .ForEach(
            instance => {
              try {
                callback?.Invoke(instance);
              }
              catch (AlreadyExistsException) {
                return;
              }
              catch (Exception ex) {
                ExceptionHandler.ShowMessageAndExit(ex);
              }
            }
          );
        ;
      });
    }
  }
}

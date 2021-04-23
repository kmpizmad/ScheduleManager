using System;
using System.Collections.Generic;
using VQG0D5.Utils.Extensions;

namespace VQG0D5.Displayer {
  public static class Displayer {
    public static void AsTable<T>(IEnumerable<T> data, string[] columnHeaders, params Func<T, object>[] valueSelectors) {
      Console.WriteLine(data.ToStringTable(columnHeaders, valueSelectors));
    }
  }
}

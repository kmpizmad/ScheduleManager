using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VQG0D5.Utils.Extensions;

namespace VQG0D5.LogManager {
  public class LogManager<T> {
    public LogManager(T data) {
      this.Data = data;
    }

    public LogManager(T[] data) {
      this.DataSet = data;
    }

    public LogManager(List<T> data) {
      this.DataList = data;
    }

    public T Data { get; protected set; }
    public T[] DataSet { get; protected set; }
    public List<T> DataList { get; protected set; }

    public void LogToConsole() {
      Console.WriteLine(this.__GetProperty());
    }

    public void LogToConsole(string[] columns, params Func<T, object>[] valueSelectors) {
      Console.WriteLine((this.__GetProperty() as IEnumerable<T>).ToStringTable(columns, valueSelectors));
    }

    private object __GetProperty() {
      PropertyInfo[] properties = this.GetType().GetProperties();

      foreach (PropertyInfo info in properties) {
        object value = info.GetValue(this, null);

        if (value == default) {
          continue;
        }

        return value;
      }

      return null;
    }
  }
}

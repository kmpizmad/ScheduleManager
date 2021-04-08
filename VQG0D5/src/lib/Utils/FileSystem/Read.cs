using System;
using System.Text;
using System.IO;
using VQG0D5.Exceptions;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace VQG0D5.Utils.FileSystem {
  public static class Read {
    /// <summary>
    /// Reads a file line by line
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public static void FromStream(string path, Action<string> callback) {
      try {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);

        while (!streamReader.EndOfStream) {
          callback?.Invoke(streamReader.ReadLine());
        }

        streamReader.Close();
      }
      catch (Exception ex) {
        ExceptionHandler.ShowMessageAndExit(ex);
      }
    }

    public static Dictionary<string, object> FromJSON(string path) {
      return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
    }
  }
}

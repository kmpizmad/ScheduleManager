using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VQG0D5.Activity;
using VQG0D5.Exceptions;

namespace VQG0D5.Utils.FileSystem {
  public static class Write {
    public static void ToFile(string path, List<BaseActivity> dailySchedule) {
      try {
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

        foreach (BaseActivity activity in dailySchedule) {
          streamWriter.WriteLine(activity.ToString());
        }

        streamWriter.Close();
      }
      catch (Exception ex) {
        ExceptionHandler.ShowMessageAndExit(ex);
      }
    }
  }
}

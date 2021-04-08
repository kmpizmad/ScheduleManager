using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VQG0D5.Exceptions {
  public static class ExceptionHandler {
    /// <summary>
    /// Logs the error message to the console and exits out of the program with code 1
    /// </summary>
    /// <param name="exception"></param>
    public static void ShowMessageAndExit(Exception exception) {
      Console.Clear();
      Console.WriteLine(exception.Message);
      Console.ReadKey();
      Environment.Exit(1);
    }
  }
}

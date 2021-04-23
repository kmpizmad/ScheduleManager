using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VQG0D5.Activity;
using Manager = VQG0D5.ScheduleManager.ScheduleManager;

namespace VQG0D5 {
  public static partial class Process {
    private static void LoadSchedules(Manager scheduleManager) {
      Regex regex = new Regex(@"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*");
      string[] schedules = Directory
        .GetFiles(Process.__outDir, "*.csv", SearchOption.AllDirectories)
        .Where(path => regex.IsMatch(path))
        .ToArray();

      foreach (string path in schedules) {
        ClassBuilder.Init(path, instance => {
          scheduleManager.Schedule.Add(
            instance as BaseActivity,
            (instance as BaseActivity).StartDate,
            (instance as BaseActivity).Id
          );
        });
      }
    }
  }
}

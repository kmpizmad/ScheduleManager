using System;
using System.Collections.Generic;
using VQG0D5.Activity;
using Manager = VQG0D5.ScheduleManager.ScheduleManager;
using Display = VQG0D5.Displayer.Displayer;

namespace VQG0D5 {
  public class Program {
    static void Main(string[] args) {
      Manager scheduleManager = Process.Init();

      Process.InitSchedules(scheduleManager);
      List<BaseActivity> dailySchedule = Process.CreateDailySchedule(scheduleManager);

      Display.AsTable(
        dailySchedule,
        new[] { "Id", "Priority", "Name", "Start", "End", "Interval" },
        activity => activity.Id,
        activity => activity.Priority,
        activity => activity.Name,
        activity => activity.StartDate.TimeOfDay,
        activity => activity.StartDate.AddMinutes(activity.MinimumTimeRequirement).TimeOfDay,
        activity => activity.Interval
      );

      Process.HandleCompletedActivities(
        scheduleManager,
        dailySchedule
      );

      Console.WriteLine("\nPress any key...");
      Console.ReadKey();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VQG0D5.Activity;
using VQG0D5.Activity.Interfaces;
using VQG0D5.Utils.Extensions;
using VQG0D5.Utils.FileSystem;
using Manager = VQG0D5.ScheduleManager.ScheduleManager;

namespace VQG0D5 {
  public static partial class Process {
    private static string __outDir;

    public static Manager Init() {
      Dictionary<string, object> config = Read.FromJSON("../../config.json");
      Dictionary<string, object> threshold = config["threshold"] as Dictionary<string, object>;
      Process.__outDir = (string)config["outDir"];
      int minimum = (int)threshold["minimum"];
      int maximum = (int)threshold["maximum"];

      DateTime minThreshold = new DateTime().AddHours(minimum);
      DateTime maxThreshold = new DateTime().AddHours(maximum);

      return new Manager(minThreshold, maxThreshold);
    }

    public static void InitSchedules(Manager scheduleManager) {
      Console.Write("File location: ");
      string path = Console.ReadLine();

      LoadSchedules(scheduleManager);
      scheduleManager.Init(path);
    }

    public static List<BaseActivity> CreateDailySchedule(Manager scheduleManager) {
      Console.Clear();

      Console.Write("Date (format: yyyy-MM-dd): ");
      string date = Console.ReadLine();

      Console.Clear();

      List<BaseActivity> dailySchedule = scheduleManager.CreateSchedule(date);
      Write.ToFile(Process.__outDir + date + ".csv", dailySchedule);

      return dailySchedule;
    }

    public static void HandleCompletedActivities(Manager scheduleManager, List<BaseActivity> activities) {
      List<BaseActivity> occasional = activities
        .Where(activity => activity is OccasionalActivity)
        .ToList();

      if (occasional.Count == 0) {
        return;
      }

      Console.Write("Completed activities (id): ");
      List<string> indentifiers = Console.ReadLine()
        .Split(',', ';', '-', '.', ' ')
        .ToList();

      indentifiers
        .ForEach(item => item.Trim());

      List<BaseActivity> completed = activities
        .Where(activity => indentifiers.Contains(activity.Id.ToString()))
        .ToList();

      scheduleManager.activityCompletedEvent += EventHandler.HandleCompletion;
      completed.ForEach(activity => scheduleManager.CompleteActivity(activity as IAutomaticSchedule));
    }
  }
}

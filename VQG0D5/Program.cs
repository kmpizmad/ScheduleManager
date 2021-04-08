using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VQG0D5.Activity;
using VQG0D5.Activity.Interfaces;
using VQG0D5.Exceptions;
using VQG0D5.Utils.Extensions;
using VQG0D5.Utils.FileSystem;
using VQG0D5.Utils.Reflection;
using Manager = VQG0D5.ScheduleManager.ScheduleManager;

namespace VQG0D5 {
  public class Program {
    static void Main(string[] args) {
      Dictionary<string, object> config = Read.FromJSON("../../config.json");
      Dictionary<string, object> threshold = config["threshold"] as Dictionary<string, object>;
      int minimum = (int)threshold["minimum"];
      int maximum = (int)threshold["maximum"];

      DateTime minThreshold = new DateTime().AddHours(minimum);
      DateTime maxThreshold = new DateTime().AddHours(maximum);

      Manager scheduleManager = new Manager(minThreshold, maxThreshold);

      Init(scheduleManager);
      HandleDailySchedule(scheduleManager);

      Console.WriteLine("\nPress any key...");
      Console.ReadKey();
    }

    private static void Init(Manager scheduleManager) {
      LoadSchedules(scheduleManager);

      Console.Write("File location: ");
      string path = Console.ReadLine();

      LoadData(path, instance => {
        scheduleManager.Activities.Add(
          instance as BaseActivity,
          (instance as BaseActivity).Priority,
          (instance as BaseActivity).Id
        );
      });

      scheduleManager.AfterInit();
    }

    private static void LoadSchedules(Manager scheduleManager) {
      Regex regex = new Regex(@"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*");
      string[] schedules = Directory
        .GetFiles("./", "*.csv", SearchOption.AllDirectories)
        .Where(path => regex.IsMatch(path))
        .ToArray();

      foreach (string path in schedules) {
        LoadData(path, instance => {
          scheduleManager.Schedule.Add(
            instance as BaseActivity,
            (instance as BaseActivity).StartDate,
            (instance as BaseActivity).Id
          );
        });
      }
    }

    private static void LoadData(string path, Action<object> callback) {
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

    private static void HandleDailySchedule(Manager scheduleManager) {
      Console.Clear();

      Console.Write("Date (format: yyyy-MM-dd): ");
      string date = Console.ReadLine();

      Console.Clear();

      List<BaseActivity> dailySchedule = scheduleManager.CreateSchedule(date);
      Write.ToFile(date + ".csv", dailySchedule);

      Console.WriteLine(
        dailySchedule.ToStringTable(
          new[] { "Id", "Priority", "Name", "Start", "End", "Interval" },
          activity => activity.Id,
          activity => activity.Priority,
          activity => activity.Name,
          activity => activity.StartDate.TimeOfDay,
          activity => activity.StartDate.AddMinutes(activity.MinimumTimeRequirement).TimeOfDay,
          activity => activity.Interval
        )
      );

      HandleCompletedActivities(scheduleManager, dailySchedule);
    }

    private static void HandleCompletedActivities(Manager scheduleManager, List<BaseActivity> activities) {
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

      scheduleManager.activityCompletedEvent += HandleCompletion;
      completed.ForEach(activity => scheduleManager.CompleteActivity(activity as IAutomaticSchedule));
    }

    private static void HandleCompletion(IAutomaticSchedule activity) {
      Console.WriteLine($"{(activity as BaseActivity).Name} has been completed!");
    }
  }
}

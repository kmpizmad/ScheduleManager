using System;
using System.Collections.Generic;
using System.Linq;
using VQG0D5.Activity;
using VQG0D5.Utils.DataStructures;

namespace VQG0D5.ScheduleManager {
  partial class ScheduleManager {
    private LinkedList<BaseActivity, DateTime, int> __schedule;
    private LinkedList<BaseActivity, Priority, int> __activities;

    /// <summary>
    /// Gets the regular activities
    /// </summary>
    /// <returns></returns>
    private LinkedList<BaseActivity, DateTime, int> __GetActivities() {
      LinkedList<BaseActivity, DateTime, int> activities = new LinkedList<BaseActivity, DateTime, int>();

      this.__activities
        .Filter(data => data is RegularActivity)
        .ForEach(activity => activities.Add(activity, activity.StartDate, activity.Id));

      return activities;
    }

    /// <summary>
    /// Gets the regular activities on a specific date
    /// </summary>
    /// <param name="date">Format: yyyy-MM-dd</param>
    /// <returns></returns>
    private LinkedList<BaseActivity, DateTime, int> __GetActivities(string date) {
      return this.__GetActivities()
        .Filter(data => data.StartDate.ToString("yyyy-MM-dd") == date);
    }

    /// <summary>
    /// Gets all unassigned activities
    /// </summary>
    /// <returns></returns>
    private LinkedList<OccasionalActivity, Priority, int> __GetUnassignedActivities() {
      LinkedList<OccasionalActivity, Priority, int> occasionalActivities = new LinkedList<OccasionalActivity, Priority, int>(false);

      this.__activities
        .Filter(data => data is OccasionalActivity && (data as OccasionalActivity).IsAssigned == false)
        .ForEach(activity => occasionalActivities.Add(activity as OccasionalActivity, activity.Priority, activity.Id));

      return occasionalActivities;
    }

    /// <summary>
    /// Gets the time windows between regular activities
    /// </summary>
    /// <param name="activities"></param>
    /// <returns>A list of time windows in minutes</returns>
    private List<int> __GetTimeWindows(BaseActivity[] activities) {
      List<int> timeWindows = new List<int>();

      for (int i = 1; i < activities.Length; i++) {
        DateTime end = activities[i - 1]
            .StartDate
            .AddMinutes(activities[i - 1].MinimumTimeRequirement);

        DateTime start = activities[i]
            .StartDate;

        timeWindows.Add((int)Math.Abs((end - start).TotalMinutes));
      }

      timeWindows.Insert(0, 0);

      return timeWindows;
    }

    /// <summary>
    /// Determines if the last activity in the list has reached the given threshold
    /// </summary>
    /// <param name="activity"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    private bool __IsReachedThreshold(List<BaseActivity> activities, DateTime threshold) {
      if (activities.Count == 0) {
        return false;
      }

      BaseActivity activity = activities[activities.Count - 1];
      DateTime start = activity.StartDate;
      int minutes = activity.MinimumTimeRequirement;

      int compared = TimeSpan.Compare(
        start.AddMinutes(minutes).TimeOfDay,
        threshold.TimeOfDay
      );

      return compared >= 0;
    }

    /// <summary>
    /// Assigns every activity
    /// </summary>
    /// <param name="date"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    private List<BaseActivity> __AssignAll(string date, DateTime threshold) {
      OccasionalActivity[] unassignedActivities = this.__GetUnassignedActivities().ToArray();
      List<BaseActivity> result = new List<BaseActivity>();

      foreach (OccasionalActivity activity in unassignedActivities) {
        DateTime d = result.Count > 0
          ? result[result.Count - 1].StartDate.AddMinutes(result[result.Count - 1].MinimumTimeRequirement)
          : DateTime.Parse(date).AddHours(threshold.Hour);

        activity.Assign(d);
        result.Add(activity);
      }

      return result;
    }

    /// <summary>
    /// Assigns activities that meets the requirements
    /// </summary>
    /// <param name="date"></param>
    /// <param name="activities"></param>
    /// <returns><returns>
    private List<BaseActivity> __AssignSome(string date, BaseActivity[] activities) {
      OccasionalActivity[] unassignedActivities = this.__GetUnassignedActivities().ToArray();
      List<int> timeWindows = this.__GetTimeWindows(activities);
      var (minThreshold, maxThreshold) = this.Threshold;

      List<BaseActivity> result = new List<BaseActivity>();

      int i = 0;
      int j = 0;

      // Set time window if first regular activity starts later then the given minimum threshold
      if (
        activities.Length > 0 &&
        TimeSpan.Compare(activities[0].StartDate.TimeOfDay, minThreshold.TimeOfDay) > 0
      ) {
        timeWindows[0] = (int)Math.Abs((activities[0].StartDate.TimeOfDay - minThreshold.TimeOfDay).TotalMinutes);
      }

      while (i < activities.Length) {
        while (
          j < unassignedActivities.Length &&
          timeWindows[i] - unassignedActivities[j].MinimumTimeRequirement >= 0 &&
          !this.__IsReachedThreshold(result, maxThreshold)
        ) {
          timeWindows[i] -= unassignedActivities[j].MinimumTimeRequirement;

          // Assign the occasional activity to a date based on it's predecessor
          DateTime d = result.Count > 0
            ? result[result.Count - 1].StartDate.AddMinutes(result[result.Count - 1].MinimumTimeRequirement)
            : DateTime.Parse(date).AddHours(minThreshold.Hour);

          unassignedActivities[j].Assign(d);
          result.Add(unassignedActivities[j]);

          j += 1;
        }

        result.Add(activities[i]);
        i += 1;
      }

      return result;
    }
  }
}

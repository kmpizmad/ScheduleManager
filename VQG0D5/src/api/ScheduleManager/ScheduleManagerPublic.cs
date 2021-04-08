using System;
using System.Collections.Generic;
using System.Linq;
using VQG0D5.Activity;
using VQG0D5.Activity.Interfaces;
using VQG0D5.Exceptions;
using VQG0D5.Utils.DataStructures;

namespace VQG0D5.ScheduleManager {
  public partial class ScheduleManager {
    public event ActivityCompleted activityCompletedEvent;

    public ScheduleManager(DateTime minThreshold, DateTime maxThreshold) {
      this.__schedule = new LinkedList<BaseActivity, DateTime, int>();
      this.__activities = new LinkedList<BaseActivity, Priority, int>(false);
      this.Threshold = Tuple.Create(minThreshold, maxThreshold);
    }

    public Tuple<DateTime, DateTime> Threshold { get; private set; }
    public LinkedList<BaseActivity, DateTime, int> Schedule => this.__schedule;
    public LinkedList<BaseActivity, Priority, int> Activities => this.__activities;

    /// <summary>
    /// Removes already existing schedules from activities list
    /// </summary>
    public void AfterInit() {
      if (this.Schedule.Head == null) {
        return;
      }

      LinkedList<BaseActivity, Priority, int> schedule = new LinkedList<BaseActivity, Priority, int>(false);
      this.Schedule.ForEach(activity => schedule.Add(activity, activity.Priority, activity.Id));

      this.__activities = schedule.Difference(this.Activities);
    }

    /// <summary>
    /// Creates a schedule for a specific day
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public List<BaseActivity> CreateSchedule(string date) {
      BaseActivity[] regularActivities = this.__GetActivities(date).ToArray();
      var (minThreshold, _) = this.Threshold;

      if (regularActivities.Length == 0) {
        return this.__AssignAll(date, minThreshold);
      }

      return this.__AssignSome(date, regularActivities);
    }

    /// <summary>
    /// Triggers an event that handles completion
    /// </summary>
    /// <param name="activity"></param>
    public void CompleteActivity(IAutomaticSchedule activity) {
      if (activity.IsCompleted) {
        throw new AlreadyCompletedException("Received activity is already completed!", activity);
      }

      activity.Complete();
      this.activityCompletedEvent?.Invoke(activity);
    }
  }
}

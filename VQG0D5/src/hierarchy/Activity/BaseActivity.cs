using System;

namespace VQG0D5.Activity {
  public abstract class BaseActivity {
    protected int _interval;

    protected BaseActivity(int id, string name, int minTimeRequirementInMinutes, Priority priority) {
      this.Id = id;
      this.Name = name;
      this.MinimumTimeRequirement = minTimeRequirementInMinutes;
      this.Priority = priority;
    }

    public int Id { get; protected set; }
    public string Name { get; protected set; }
    public int MinimumTimeRequirement { get; protected set; }
    public Priority Priority { get; protected set; }
    public DateTime StartDate { get; protected set; }

    public abstract ActivityType Interval { get; }

    public override string ToString() {
      return $"{this.Id};{this.StartDate};{this.Name};{this.MinimumTimeRequirement};{this.Priority};{(int)this.Interval}";
    }

    /// <summary>
    /// Determines if this activity will be held on a specific day
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public virtual bool HasActivity(DateTime date) {
      int days = (int)Math.Abs((this.StartDate - date).TotalDays);
      return days % this._interval == 0;
    }
  }
}

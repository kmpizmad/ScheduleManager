using System;
using VQG0D5.Activity.Interfaces;

namespace VQG0D5.Activity {
  public class OccasionalActivity : BaseActivity, IAutomaticSchedule {
    public OccasionalActivity(int id, string name, int minTimeRequirementInMinutes, Priority priority)
      : base(id, name, minTimeRequirementInMinutes, priority) {
    }

    public OccasionalActivity(int id, DateTime date, string name, int minTimeRequirementInMinutes, Priority priority)
      : base(id, name, minTimeRequirementInMinutes, priority) {
      this.StartDate = date;
      this.IsAssigned = true;
    }

    public bool IsAssigned { get; protected set; }
    public bool IsCompleted { get; protected set; } = false;
    public override ActivityType Interval => ActivityType.Occasional;

    public override bool HasActivity(DateTime date) {
      return this.StartDate.Date == date.Date;
    }

    public void Assign(DateTime date) {
      this.StartDate = date;
      this.IsAssigned = true;
    }

    public void Complete() {
      this.IsCompleted = true;
    }
  }
}

using System;

namespace VQG0D5.Activity {
  public abstract class RegularActivity : BaseActivity {
    protected RegularActivity(int id, DateTime startDate, string name, int minTimeRequirementInMinutes, int interval, Priority priority) : base(id, name, minTimeRequirementInMinutes, priority) {
      this.StartDate = startDate;
      this._interval = interval;
    }

    public override ActivityType Interval => (ActivityType)this._interval;
  }
}

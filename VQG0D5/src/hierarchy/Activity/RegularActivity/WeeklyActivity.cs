using System;

namespace VQG0D5.Activity {
  public class WeeklyActivity : RegularActivity {
    public WeeklyActivity(int id, DateTime startDate, string name, int minTimeRequirementInMinutes, Priority priority)
      : base(id, startDate, name, minTimeRequirementInMinutes, 7, priority) {
    }
  }
}

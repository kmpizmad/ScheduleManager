using System;

namespace VQG0D5.Activity {
  public class DailyActivity : RegularActivity {
    public DailyActivity(int id, DateTime startDate, string name, int minTimeRequirementInMinutes, Priority priority)
      : base(id, startDate, name, minTimeRequirementInMinutes, 1, priority) {
    }
  }
}

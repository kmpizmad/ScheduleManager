using System;

namespace VQG0D5.Activity {
  public class YearlyActivity : RegularActivity {
    private static readonly int INTERVAL = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;

    public YearlyActivity(int id, DateTime startDate, string name, int minTimeRequirementInMinutes, Priority priority)
      : base(id, startDate, name, minTimeRequirementInMinutes, INTERVAL, priority) {
    }
  }
}

using VQG0D5.Activity.Interfaces;

namespace VQG0D5.Activity {
  public delegate void ActivityCompleted(IAutomaticSchedule activity);
  public delegate void TraverseHandler<T>(T activity);
}

using System;
using VQG0D5.Activity;
using VQG0D5.Activity.Interfaces;

namespace VQG0D5 {
  public static class EventHandler {
    public static void HandleCompletion(IAutomaticSchedule activity) {
      Console.WriteLine($"{(activity as BaseActivity).Name} has been completed!");
    }
  }
}

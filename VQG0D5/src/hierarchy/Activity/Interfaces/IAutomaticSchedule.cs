using System;

namespace VQG0D5.Activity.Interfaces {
  public interface IAutomaticSchedule {
    bool IsAssigned { get; }
    bool IsCompleted { get; }

    /// <summary>
    /// Assigns a date to this instance
    /// </summary>
    /// <param name="date"></param>
    void Assign(DateTime date);

    /// <summary>
    /// Handles the completion logic of this instance
    /// </summary>
    void Complete();
  }
}

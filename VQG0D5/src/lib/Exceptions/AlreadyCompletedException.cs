using System;
using System.Runtime.Serialization;

namespace VQG0D5.Exceptions {
  public class AlreadyCompletedException : CustomBaseException {
    public AlreadyCompletedException() {
    }

    public AlreadyCompletedException(string message) : base(message) {
    }

    public AlreadyCompletedException(string message, object data) : base(message, data) {
    }

    public AlreadyCompletedException(string message, Exception innerException) : base(message, innerException) {
    }

    public AlreadyCompletedException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
  }
}

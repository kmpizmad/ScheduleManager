using System;
using System.Runtime.Serialization;

namespace VQG0D5.Exceptions {
  public class NotFoundException : CustomBaseException {
    public NotFoundException() {
    }

    public NotFoundException(string message) : base(message) {
    }

    public NotFoundException(string message, object data) : base(message, data) {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) {
    }

    public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
  }
}

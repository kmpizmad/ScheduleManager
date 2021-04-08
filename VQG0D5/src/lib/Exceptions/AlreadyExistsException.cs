using System;
using System.Runtime.Serialization;

namespace VQG0D5.Exceptions {
  public class AlreadyExistsException : CustomBaseException {
    public AlreadyExistsException() {
    }

    public AlreadyExistsException(string message) : base(message) {
    }

    public AlreadyExistsException(string message, object data) : base(message, data) {
    }

    public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) {
    }

    public AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
  }
}

using System;
using System.Runtime.Serialization;

namespace VQG0D5.Exceptions {
  public abstract class CustomBaseException : ApplicationException {
    protected object _data;

    protected CustomBaseException() {
    }

    protected CustomBaseException(string message) : base(message) {
    }

    protected CustomBaseException(string message, object data) : base(message) {
      this._data = data;
    }

    protected CustomBaseException(string message, Exception innerException) : base(message, innerException) {
    }

    protected CustomBaseException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }

    public object RespectiveData => this._data;
  }
}

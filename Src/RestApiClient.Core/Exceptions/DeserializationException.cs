using System;

namespace RestApiClient.Core.Exceptions
{
    public class DeserializationException : Exception
    {
        public DeserializationException() : base()
        {
        }

        public DeserializationException(string message) : base(message)
        {
        }

        public DeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

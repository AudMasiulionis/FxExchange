using System;

namespace FxExchange.Domain.Exceptions
{
    public class RateNotFoundException : Exception
    {
        public RateNotFoundException() : base()
        {
        }

        public RateNotFoundException(string message) : base(message)
        {
        }

        public RateNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

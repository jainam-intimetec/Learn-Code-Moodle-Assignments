using System;

namespace ATMApp.Exceptions
{
    public class ATMOperationException : Exception
    {
        public ATMOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
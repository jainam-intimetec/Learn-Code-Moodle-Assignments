using System;

namespace ATMApp.Exceptions
{
    public class NetworkConnectionException : Exception
    {
        public NetworkConnectionException(string message) : base(message) { }
    }
}

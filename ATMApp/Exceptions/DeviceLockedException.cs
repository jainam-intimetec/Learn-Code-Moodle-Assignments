using System;

namespace ATMApp.Exceptions
{
    public class DeviceLockedException : Exception
    {
        public DeviceLockedException(string message) : base(message) { }
    }
}

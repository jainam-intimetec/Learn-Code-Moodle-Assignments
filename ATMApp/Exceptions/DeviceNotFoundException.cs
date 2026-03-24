using System;

namespace ATMApp.Exceptions
{
    public class DeviceNotFoundException : Exception
    {
        public DeviceNotFoundException(string message) : base(message) { }
    }
}

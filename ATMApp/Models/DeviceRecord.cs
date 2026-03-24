namespace ATMApp.Models
{
    public class DeviceRecord
    {
        public DeviceStatus Status { get; set; } = DeviceStatus.Active;
        public WifiConnection WifiConnection { get; set; } = WifiConnection.Connected;
    }

    public enum DeviceStatus
    {
        Active,
        Suspended
    }

    public enum WifiConnection
    {
        Connected,
        Disconnected
    }
}

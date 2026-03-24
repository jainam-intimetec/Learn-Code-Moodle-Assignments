using ATMApp.Exceptions;
using ATMApp.Models;

namespace ATMApp.Services
{
    public class ATMService
    {
        private const string DEV1 = "DEV1";

        public void Withdraw(string accountId, double amount)
        {
            var handle = GetHandle(DEV1);
            if (handle == DeviceHandle.INVALID)
            {
                throw new DeviceNotFoundException($"Device {DEV1} not found.");
            }

            var record = RetrieveDeviceRecord(handle);

            if (record.Status == DeviceStatus.Suspended)
            {
                throw new DeviceLockedException("ATM device is suspended and cannot process withdrawals.");
            }

            if (record.WifiConnection != WifiConnection.Connected)
            {
                throw new NetworkConnectionException("ATM has no network connection.");
            }

            var balance = GetBalance(accountId);
            if (balance < amount)
            {
                throw new InsufficientFundsException("Insufficient funds for withdrawal.");
            }

            DispenseCash(handle, amount);
        }

        private DeviceHandle GetHandle(string deviceId) => DeviceHandle.INVALID;

        private DeviceRecord RetrieveDeviceRecord(DeviceHandle handle) => new DeviceRecord();

        private double GetBalance(string accountId) => 0.0;

        private void DispenseCash(DeviceHandle handle, double amount) { }
    }
}
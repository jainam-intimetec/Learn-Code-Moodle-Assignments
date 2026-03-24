using ATMApp.Exceptions;
using ATMApp.Services;

namespace ATMApp.Controllers
{
    public class ATMDeviceController
    {
        private readonly ATMService _atmService;

        public ATMDeviceController(ATMService atmService)
        {
            _atmService = atmService;
        }

        public void Withdraw(string accountId, double amount)
        {
            try
            {
                _atmService.Withdraw(accountId, amount);
            }
            catch (DeviceNotFoundException ex)
            {
                throw new ATMOperationException("Device error: " + ex.Message, ex);
            }
            catch (DeviceLockedException ex)
            {
                throw new ATMOperationException("Device locked: " + ex.Message, ex);
            }
            catch (NetworkConnectionException ex)
            {
                throw new ATMOperationException("Network error: " + ex.Message, ex);
            }
            catch (InsufficientFundsException ex)
            {
                throw new ATMOperationException("Funds error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new ATMOperationException("Unexpected error during withdrawal.", ex);
            }
        }
    }
}

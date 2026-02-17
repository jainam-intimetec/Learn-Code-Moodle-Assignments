using System;
using System.Collections.Generic;

namespace Payment.Processing
{
    public class PaymentProcessor
    {
        private static readonly decimal MinAmount = 0.01m;
        private const int MaxRetries = 2;

        private const string PaymentSuccess = "Payment successful";
        private const string PaymentFailed  = "Payment failed";

        private readonly ILogger logger;
        private readonly INotificationService notifier;
        private readonly Dictionary<string, PaymentRecord> history;

        public PaymentProcessor(ILogger logger, INotificationService notifier)
        {
            this.logger = logger;
            this.notifier = notifier;
            history = new Dictionary<string, PaymentRecord>();
        }

        public PaymentResult Process(PaymentRequest request)
        {
            Validate(request);

            var attempt = 0;

            while (attempt < MaxRetries)
            {
                try
                {
                    Execute(request);
                    Record(request);
                    NotifySuccess(request);

                    return new PaymentResult(
                        true,
                        PaymentSuccess,
                        GenerateId()
                    );
                }
                catch (PaymentException)
                {
                    attempt++;
                    logger.Log("Retry attempt: " + attempt);
                }
            }

            return new PaymentResult(false, PaymentFailed, null);
        }

        private void Validate(PaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
                throw new ArgumentException("Customer ID required");

            if (request.Amount == null || request.Amount < MinAmount)
                throw new ArgumentException("Invalid amount");
        }

        private void Execute(PaymentRequest request)
        {
            logger.Log("Executing payment of " + request.Amount);

            if (request.Amount > 5000m)
                throw new PaymentException("Limit exceeded");
        }

        private void Record(PaymentRequest request)
        {
            history[GenerateId()] = new PaymentRecord(
                request.CustomerId,
                request.Amount,
                DateTime.UtcNow
            );
        }

        private void NotifySuccess(PaymentRequest request)
        {
            notifier.Send(
                request.CustomerId,
                "Payment of " + request.Amount + " processed"
            );
        }

        private string GenerateId()
        {
            return "TXN-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
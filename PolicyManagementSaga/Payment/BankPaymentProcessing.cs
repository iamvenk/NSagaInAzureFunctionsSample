using System;
using System.Collections.Generic;
using System.Text;
using NSaga;
using PolicyEntities;

namespace PolicyManagementSaga.Payment
{
    public class BankPaymentProcessing : ISagaMessage
    {
        public Guid CorrelationId { get; set; }

        public BankPayment BankPayment { get; set; }

        public BankPaymentProcessing(Guid correlationId, BankPayment bankPayment)
        {
            this.CorrelationId = correlationId;
            this.BankPayment = bankPayment;
        }
    }
}

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

        public BankPaymentProcessing(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
    }
}

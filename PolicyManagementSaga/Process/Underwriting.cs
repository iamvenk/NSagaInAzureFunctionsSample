using System;
using System.Collections.Generic;
using System.Text;
using NSaga;

namespace PolicyManagementSaga.Process
{
    public class Underwriting : ISagaMessage
    {
        public Guid CorrelationId { get; set; }

        public Underwriting(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
    }
}

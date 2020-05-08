using System;
using System.Collections.Generic;
using System.Text;
using NSaga;

namespace PolicyManagementSaga.Process
{
    public class PolicyCreation : ISagaMessage
    {
        public Guid CorrelationId { get; set; }

        public PolicyCreation(Guid guid)
        {
            this.CorrelationId = guid;
        }
    }
}

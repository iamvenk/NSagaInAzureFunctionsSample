using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using NSaga;
using PolicyEntities;

namespace PolicyManagementSaga.Creation
{
    public class PolicyCreationSagaData
    {
        public NewPolicyRequest Request { get; set; }
        public Policy Policy { get; set; }
        public Invoice Invoice { get; set; }
        public string PaymentConfirmationNumber { get; set; }
    }
}
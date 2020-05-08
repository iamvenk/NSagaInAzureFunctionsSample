using System;
using System.Collections.Generic;
using System.Text;
using PolicyEntities;
using NSaga;

namespace PolicyManagementSaga.Creation
{
    public class NewPolicyRequest : IInitiatingSagaMessage
    {
        Guid ISagaMessage.CorrelationId { get; set; }
        
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Person Insured { get; set; }
        public Person Holder { get; set; }
        public List<Coverage> Coverages { get; set; }
        public BankPayment Payment { get; set; }
    }
}

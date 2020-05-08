using System;
using System.Collections.Generic;
using System.Text;
using PolicyEntities;
using System.Linq;

namespace PolicyBusiness
{
    public class PolicyManagement
    {
        public Policy CreatePolicy(Policy data)
        {
            data.IssuedOn = DateTime.Now;
            data.Id = NewID;

            return data;
        }

        public void SaveInvoice(Invoice invoice)
        {
            invoice.Number = NewID;
        }

        public List<string> UnderwriteNewPolicy(Policy policy)
        {
            List<string> errors = new List<string>();

            if (!(policy.Coverages.Count > 0 && policy.Coverages.Count <= 3))
                errors.Add("Number of coverages should be 1 to 3");

            return errors;
        }

        public Invoice GenerateInvoice(Policy policy)
        {
            List<Invoice> invoices = new List<Invoice>();

            return new Invoice()
            {
                PolicyId = policy.Id,
                DueOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                Status = InvoiceStatus.Open,
                Amount = policy.Coverages.Sum(c => c.Premium)
            };
        }

        public List<string> ValidateNewPolicyRequest(Policy policy)
        {
            return PolicyValidator.ValidateNewPolicyRequest(policy);
        }

        private string NewID
        {
            get
            {
                return (new Random()).Next().ToString();
            }
        }
    }
}

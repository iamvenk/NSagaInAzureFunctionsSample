using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolicyEntities;

namespace PolicyBusiness
{
    public class PolicyUnderWriter
    {
        public static List<string> UnderwriteNewPolicy(Policy policy)
        {
            List<string> errors = new List<string>();

            if (!(policy.Coverages.Count > 0 && policy.Coverages.Count <= 3))
                errors.Add("Number of coverages should be 1 to 3");

            return errors;
        }

        public static Invoice GenerateInvoice(Policy policy)
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

    }
}

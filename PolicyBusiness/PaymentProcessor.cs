using System;
using System.Collections.Generic;
using System.Text;
using PolicyBusiness;
using PolicyEntities;

namespace PolicyBusiness
{
    public class PaymentProcessor
    {
        public BankPaymentResponse InvoicePayment(Invoice invoice, BankPayment bankPayment)
        {
            var response = new BankPaymentResponse();
            List<string> errors = new List<string>();

            if (bankPayment == null)
            {
                response.Errors.Add("Payment details invalid");
                return response;
            }

            if(bankPayment.Bank == "XYZ")
            {
                response.Errors.Add("Payment through bank XYZ is not allowed");
                return response;
            }

            invoice.ClosedOn = DateTime.Now;
            invoice.Status = InvoiceStatus.Closed;
            response.PaymentConfirmatitonNumber = $"{bankPayment.Bank}{DateTime.Now:ssyymmhh}";

            return response;
        }

        public class BankPaymentResponse
        {
            public string PaymentConfirmatitonNumber { get; set; }
            public List<string> Errors { get; set; }

            public BankPaymentResponse()
            {
                Errors = new List<string>();
            }
        }
    }
}

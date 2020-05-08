using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyEntities
{
    public class Invoice
    {
        public string Number { get; set; }
        public string PolicyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueOn { get; set; }
        public DateTime ClosedOn { get; set; }
        public InvoiceStatus Status { get; set; }
    }

    public enum InvoiceStatus
    {
        Open,
        Closed,
        Cancelled
    }
}
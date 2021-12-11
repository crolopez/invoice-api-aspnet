using System;

namespace InvoiceApi.Core.Domain.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public string Supplier { get; set; }
        public DateTime DateIssued { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
using System;

namespace InvoiceApi.Models
{
    public class Invoice
    {
        public long InvoiceId { get; set; }
        public string Supplier { get; set; }
        public DateTime DateIssued { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
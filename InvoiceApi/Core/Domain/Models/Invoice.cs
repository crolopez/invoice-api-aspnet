using System;

namespace InvoiceApi.Core.Domain.Models
{
    public class Invoice
    {
        public string invoiceId { get; set; }
        public string supplier { get; set; }
        public DateTime dateIssued { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
    }
}
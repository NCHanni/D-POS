using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class credit_memo
    {
        [Key]
        public string document_no { get; set; }
        public DateTime? document_date { get; set; }
        public string customer_no { get; set; }
        public string customer_name { get; set; }
        public string external_document_no { get; set; }
        public decimal amount { get; set; }
    }
}

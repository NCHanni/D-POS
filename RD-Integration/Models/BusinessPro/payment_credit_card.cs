using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class payment_credit_card
    {
        [Key]
        public long id { get; set; }
        public string sale_code { get; set; }
        public string type_code { get; set; }
        public string type_name { get; set; }
        public string card_no { get; set; }
        public string card_holder { get; set; }
        public string bank_name { get; set; }
        public string approval_code { get; set; }
        public decimal amount { get; set; }
        public string payment_code { get; set; }
    }
}

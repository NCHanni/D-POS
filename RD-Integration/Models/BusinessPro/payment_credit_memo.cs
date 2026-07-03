using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class payment_credit_memo
    {
        [Key]
        public long id { get; set; }
        public string sale_code { get; set; }
        public string memo_code { get; set; }
        public decimal amount { get; set; }
        public string payment_code { get; set; }
    }
}

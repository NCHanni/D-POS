using System;
using System.ComponentModel.DataAnnotations;

namespace RD_INTEGRATION.Models.BusinessPro
{
    public class payment_cash
    {
        [Key]
        public long id { get; set; }
        public string sale_code { get; set; }
        public decimal amount { get; set; }
        public decimal change { get; set; }
        public string payment_code{ get; set; }
    }
}
